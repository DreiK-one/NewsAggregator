using NewsAggregator.Core.Interfaces.InterfacesCQS;
using System.Text.RegularExpressions;


namespace NewsAggregator.WebAPI.Validators
{
    public class ValidationMethodsCQS : IValidationMethodsCQS
    {
        private readonly IAccountServiceCQS _accountServiceCQS;

        public ValidationMethodsCQS(IAccountServiceCQS accountServiceCQS)
        {
            _accountServiceCQS = accountServiceCQS;
        }

        public bool HasLowerCase(string pw)
        {
            var lowercase = new Regex("[a-z]+");
            return lowercase.IsMatch(pw);
        }

        public bool HasUpperCase(string pw)
        {
            var uppercase = new Regex("[A-Z]+");
            return uppercase.IsMatch(pw);
        }

        public bool HasSymbol(string pw)
        {
            var symbol = new Regex("(\\W)+");
            return symbol.IsMatch(pw);
        }

        public bool HasDigit(string pw)
        {
            var digit = new Regex("(\\d)+");
            return digit.IsMatch(pw);
        }

        public bool CheckIsEmailExists(string email)
        {
            if (email != null)
            {
                var result = _accountServiceCQS
                    .ValidateIsEmailExists(email);
                return !Convert.ToBoolean(result.Result);
            }
            return false;
        }

        public bool CheckIsEmailDoesntExists(string email)
        {
            if (email != null)
            {
                var result = _accountServiceCQS
                    .ValidateIsEmailExists(email);
                return Convert.ToBoolean(result.Result);
            }
            return false;
        }         

        public bool CheckIsNicknameExists(string nickname)
        {
            if (nickname!= null)
            {
                var result = _accountServiceCQS
                    .ValidateIsNicknameExists(nickname);
                return !Convert.ToBoolean(result.Result);
            }
            return false;    
        }

        public bool CheckIsNicknameOrEmailContainsAdminWord(string adminWord)
        {
            if (adminWord.ToLowerInvariant().Contains("admin"))
            {
                return false;
            }
            return true;
        }
    }
}
