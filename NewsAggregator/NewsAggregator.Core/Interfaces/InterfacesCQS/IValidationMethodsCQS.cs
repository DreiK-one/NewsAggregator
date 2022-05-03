namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface IValidationMethodsCQS
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
