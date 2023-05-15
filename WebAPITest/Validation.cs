using WebAPITest.Interfaces;
using System.Text.RegularExpressions;

namespace WebAPITest
{
    public class Validation : IValidationService
    {
        public bool ValidDNI(string DNI)
        {
            if (DNI.Length != 9) return false;

            var numbersDNI = DNI.Substring(0, DNI.Length - 1);
            var letterDNI = DNI.Substring(DNI.Length - 1, 1);

            var integerNumberDNI = 0;
            try
            {
                integerNumberDNI = int.Parse(numbersDNI);
            }
            catch
            {
                return false;
            }

            return CalculateDNILetter(integerNumberDNI) == letterDNI;
        }

        private string CalculateDNILetter(int numbersDNI)
        {
            string[] letterList = { "T", "R", "W", "A", "G", "M", "Y", "F", "P", "D", "X", "B", "N", "J", "Z", "S", "Q", "V", "H", "L", "C", "K", "E" };
            var mod = numbersDNI % 23;
            return letterList[mod];
        }
        public bool ValidAge(int? age)
        {
            return age >= 18;
        }

        public bool ValidateEmail(string email)
        {
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }
    }
}