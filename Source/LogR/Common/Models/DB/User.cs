namespace LogR.Common.Models.DB
{
    public class User
    {
        public long ID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool? AllowAdminOperations { get; set; }
    }
}
