namespace WebAPITest.Interfaces
{
    public interface IValidationService
    {
        bool ValidDNI(string DNI);
        bool ValidAge(int? age);
        bool ValidateEmail(string email);
    }
}