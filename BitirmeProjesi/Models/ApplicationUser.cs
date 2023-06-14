using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using System.Xml.Linq;

namespace BitirmeProjesi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            IsActive = true;
            Permission = 0;
        }

        [Display(Name = "İsim")]
        [Required(ErrorMessage = "İsim Giriniz")]
        public string? Name { get; set; }
        [Display(Name = "Maaş")]
        [Required(ErrorMessage = "Maaş Girin")]
        public int? Salary { get; set; }
        [Display(Name = "İşe Giriş Tarihi")]
        [Required(ErrorMessage = "Tarih Girin")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Çıkış Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        public int Permission { get; set; }

        public virtual List<Request>? Requests { get; set; }
        public IEnumerable<EmployeeCourses>? EmployeeCourses { get; set; }


    }
}
