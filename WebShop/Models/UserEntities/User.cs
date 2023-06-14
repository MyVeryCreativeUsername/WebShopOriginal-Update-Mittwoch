using System.Collections.Generic;

namespace WebShop.Models.UserEntities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }

        public Address Address { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();





    }
}
