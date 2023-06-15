using System.ComponentModel.DataAnnotations;
namespace BitirmeProjesi.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        [Display(Name = "Departman Adı")]
        public string? DepartmentName { get; set; }
        public virtual List<ApplicationRole>? Roles { get; set; }
    }
}