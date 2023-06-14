namespace BitirmeProjesi.Models
{
    public class Course
    {
        public Course() { }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Application { get; set; }
        public string? Link { get; set; }
        public IEnumerable<EmployeeCourses>? EmployeeCourses { get; set; }
    }
}
