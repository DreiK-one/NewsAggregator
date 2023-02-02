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

        public static class Application
        {
            public static readonly string PageSize = "ApplicationVariables:PageSize";
            public static readonly string Salt = "ApplicationVariables:Salt";
        }
    }
}
