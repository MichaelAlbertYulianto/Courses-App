namespace Courses_App.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }

    }
    public class CourseWithSelected : Course
    {
        public bool IsSelected { get; set; }
    }
}
