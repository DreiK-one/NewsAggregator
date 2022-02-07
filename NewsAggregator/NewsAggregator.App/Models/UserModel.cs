namespace NewsAggregator.App.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string FullName 
        {
            get
            {
                return FirstName + " " + LastName;
            }
        } 
    }
}
