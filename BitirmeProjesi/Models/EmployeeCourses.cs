using System.ComponentModel.DataAnnotations.Schema;

namespace BitirmeProjesi.Models
{
    public class EmployeeCourses
    {
        public EmployeeCourses()
        {
            IsActive = true;
        }
        public int Id { get; set; }

        public ApplicationUser? Employee { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? EmployeeId { get; set; }

        public Course? Course { get; set; }
        [ForeignKey("Project")]
        public int? CourseId { get; set; }

        public bool IsActive { get; set; }

    }
}
