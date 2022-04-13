using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces
{
    public interface IValidationMethods
    {
        bool HasLowerCase(string pw);
        bool HasUpperCase(string pw);
        bool HasSymbol(string pw);
        bool HasDigit(string pw);
        bool CheckIsEmailExists(string email);
        bool CheckIsEmailDoesntExists(string email);
        bool CheckIsNicknameExists(string nickname);
        bool CheckIsNicknameOrEmailContainsAdminWord(string adminWord);
    }
}
