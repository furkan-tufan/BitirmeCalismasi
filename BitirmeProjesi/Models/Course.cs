using System.ComponentModel.DataAnnotations;

namespace BitirmeProjesi.Models
{
    public class Course
    {
        public Course() { }
        [Display(Name = "Kurs ID")]
        public int Id { get; set; }
        [Display(Name = "Kurs Adı")]
        public string? Name { get; set; }
        [Display(Name = "Eğitim Linki")]
        public string? Link { get; set; }
        public IEnumerable<EmployeeCourses>? EmployeeCourses { get; set; }
    }
}
