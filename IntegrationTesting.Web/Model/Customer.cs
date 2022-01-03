using System.ComponentModel.DataAnnotations;

namespace IntegrationTesting.Web.Model
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
