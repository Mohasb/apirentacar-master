using Microsoft.AspNetCore.Mvc;
using WebAPITest.Data;
using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using WebAPITest.Models.CIient;
using Microsoft.AspNetCore.Authorization;

namespace WebAPITest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly JwtSettings jwtSettings;
        private readonly IClientService _client;
        private readonly IValidationService _validation;
        public ClientController(ILogger<ClientController> logger, IOptions<JwtSettings> options, IClientService client, IValidationService validation)
        {
            _logger = logger;
            jwtSettings = options.Value;
            _client = client;
            _validation = validation;
        }
        
        [HttpGet(Name = "GetClients")]
        public Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return _client.GetAllClients();
        }

        [Authorize]
        [HttpGet("{clientDNI}", Name = "GetClient")]
        public ActionResult<Client> GetClient(string clientDNI)
        {
            var client = _client.GetOneClient(clientDNI).Result.Value!;
            return client.DNI == null
                ? NotFound()
                : client;
        }

        [HttpPost]
        public ActionResult<Client> Post(Client client)
        {
            if (!_validation.ValidDNI(client.DNI!) || _client.ClientExist(client.DNI!).Result || !_validation.ValidAge(client.Age) || !_validation.ValidateEmail(client.Email!)) return BadRequest();

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(client.Password);
            client.Password = passwordHash;
            _client.AddClient(client);

            var user = _client.GetOneClient(client.DNI!).Result.Value!;
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(this.jwtSettings.securitykey!);
            var tokendesc = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(
                    new Claim[]{new Claim(ClaimTypes.Name, user.DNI!)}
                ),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenhandler.CreateToken(tokendesc);
            string finaltoken = tokenhandler.WriteToken(token);

            return Ok(new ClientToken(user.DNI!, user.Name!, user.Surname!, user.Age, user.Email!, user.Password!, user.Phone, user.Address!, user.CP, user.City!, user.Country!, finaltoken));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(ClientCred userCred)
        {
            if(!await _client.ClientExist(userCred.dni!))
                return Unauthorized();
            // Generar Token
            var user = _client.GetOneClient(userCred.dni!).Result.Value!;
            Console.WriteLine(user.Password);
            if(!BCrypt.Net.BCrypt.Verify(userCred.password, user.Password)) return BadRequest();
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(this.jwtSettings.securitykey!);
            var tokendesc = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(
                    new Claim[]{new Claim(ClaimTypes.Name, user.DNI!)}
                ),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenhandler.CreateToken(tokendesc);
            string finaltoken = tokenhandler.WriteToken(token);

            return Ok(new ClientToken(user.DNI!, user.Name!, user.Surname!, user.Age, user.Email!, user.Password!, user.Phone, user.Address!, user.CP, user.City!, user.Country!, finaltoken));
        }

        [HttpPut("{clientDNI}")]
        public async Task<IActionResult> Put(string clientDNI, Client newClient)
        {
            if(!await _client.ClientExist(clientDNI)) return BadRequest();
            
            var client = _client.GetOneClient(clientDNI).Result.Value!;
            
            if(newClient.Name != null && newClient.Name != "string") 
                client.Name = newClient.Name;
            if(newClient.Surname != null && newClient.Surname != "string") 
                client.Surname = newClient.Surname;
            if(_validation.ValidAge(newClient.Age)) 
                client.Age = newClient.Age;
            if(newClient.Email != null && _validation.ValidateEmail(newClient.Email))
                client.Email = newClient.Email;
            if(newClient.Password != null && !BCrypt.Net.BCrypt.Verify(newClient.Password, client.Password))
                client.Password = BCrypt.Net.BCrypt.HashPassword(newClient.Password);
            if(newClient.Phone >= 0)
                client.Phone = newClient.Phone;
            if(newClient.Address != null)
                client.Address = newClient.Address;
            if(newClient.CP >= 0)
                client.CP = newClient.CP;
            if(newClient.City != null)
                client.City = newClient.City;
            if(newClient.Country != null)
                client.Country = newClient.Country;

            _client.UpdateClient(client);

            return new CreatedAtRouteResult("GetClient", new { clientDNI = client.DNI }, client);
        }

        [HttpDelete("{clientDNI}")]
        public async Task<ActionResult<Client>> Delete(string clientDNI)
        {
            if(!await _client.ClientExist(clientDNI)) return NotFound();

            var client = _client.GetOneClient(clientDNI).Result.Value!;

            _client.DeleteClient(client);

            return client;
        }
    }
}