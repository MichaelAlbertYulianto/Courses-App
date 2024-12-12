namespace Courses_App.Models
{
    public class Enrollment
    {
        public int enrollmentId { get; set; }
        public int instructorId { get; set; }
        public int courseId { get; set; }
        public string enrolledAt { get; set; }
    }

    public class EnrollmentWithSelected : Enrollment
    {
        public bool IsSelected { get; set; }
        public string fullName { get; set; }
        public string Name { get; set; }
    }
}
