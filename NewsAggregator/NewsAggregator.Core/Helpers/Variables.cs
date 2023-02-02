namespace NewsAggregator.Core.Helpers
{
    public static class Variables
    {
        public static class Roles
        {
            public static readonly string User = "USER";
            public static readonly string Admin = "Admin";
            public static readonly string Anonimous = "Anonimous";
        }

        public static class ConfigurationFields
        {
            public static readonly string PageSize = "ApplicationVariables:PageSize";
            public static readonly string Salt = "ApplicationVariables:Salt";
            public static readonly string Secret = "AppSettings:Secret";
        }

        public static class UserFields
        {
            public static readonly string Email = "Email";
            public static readonly string NormalizedEmail = "NormalizedEmail";
            public static readonly string Nickname = "Nickname";
            public static readonly string NormalizedNickname = "NormalizedNickname";
            public static readonly string PasswordHash = "PasswordHash";
        }

        public static class IdOfNewsSources
        {
            public const string Onliner = "F2FB2A60-C1DE-4DA5-B047-0871D2D677B5";
            public const string Goha = "F2FB2A60-C1DE-4DA5-B047-0871D2D677B4";
            public const string Shazoo = "C13088A4-9467-4FCE-9EF7-3903425F1F81";
        }

        public static class ArticleFields
        {
            public static readonly string Coefficient = "Coefficient";
        }

        public static class RoleFields
        {
            public static readonly string RoleId = "RoleId";
        }

        public static class Categories
        {
            public static readonly string Games = "Games";
        }

        public static class RefreshTokens
        {
            public static readonly string Revoked = "Revoked";
            public static readonly string RevokedByIp = "RevokedByIp";
            public static readonly string ReasonOfRevoke = "ReasonOfRevoke";
            public static readonly string ReplacedByToken = "ReplacedByToken";
        }
    }
}
