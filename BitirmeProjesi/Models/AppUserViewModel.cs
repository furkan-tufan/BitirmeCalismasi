using System.ComponentModel.DataAnnotations;
namespace Bitirme.Models
{
    public class AppUserViewModel
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}