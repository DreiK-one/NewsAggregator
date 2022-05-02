namespace NewsAggregator.Data.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public virtual IEnumerable<UserRole> UserRoles { get; set; }
    }
}
