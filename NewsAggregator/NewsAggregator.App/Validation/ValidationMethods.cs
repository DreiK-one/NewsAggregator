using NewsAggregator.App.Models;
using NewsAggregator.Core.Interfaces;
using System.Text.RegularExpressions;

namespace NewsAggregator.App.Validation
{
    public class ValidationMethods : IValidationMethods
    {
        private readonly IAccountService _accountService;

        public ValidationMethods(IAccountService accountService)
        {
            _accountService = accountService;
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
                var result = _accountService.ValidateIsEmailExists(email);
                return !result;
            }
            return false;
        }

        public bool CheckIsEmailDoesntExists(string email)
        {
            if (email != null)
            {
                var result = _accountService.ValidateIsEmailExists(email);
                return result;
            }
            return false;
        }
            

        public bool CheckIsNicknameExists(string nickname)
        {
            if (nickname!= null)
            {
                var result = _accountService.ValidateIsNicknameExists(nickname);
                return !result;
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
