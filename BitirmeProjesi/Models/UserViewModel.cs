using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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
        [Display(Name = "İsim")]
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public IEnumerable<string>? Roles { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "İşe Giriş Tarihi")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Maaş")]
        public int? Salary { get; set; }
        [Display(Name = "İzin Hakkı")]
        public int Permission { get; set; }
        [Display(Name = "Çıkış Tarihi")]
        public DateTime? EndDate { get; set; }
        public string PhoneNumber { get; set; }
    }
}