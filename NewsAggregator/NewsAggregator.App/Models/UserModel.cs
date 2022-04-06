namespace NewsAggregator.App.Models
{
    public class UserModel : BaseModel
    {
        public string Email { get; set; }
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
