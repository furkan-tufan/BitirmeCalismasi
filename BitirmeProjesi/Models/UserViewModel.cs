using System.ComponentModel.DataAnnotations;
namespace Bitirme.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            IsActive = true;
            Permission = 0;
        }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public IEnumerable<string>? Roles { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public int? Salary { get; set; }
        public int Permission { get; set; }
        public DateTime? EndDate { get; set; }
        public string PhoneNumber { get; set; }
    }
}