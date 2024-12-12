namespace Courses_App.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class CategoryWithSelection : Category
    {
        public bool IsSelected { get; set; }
    }
}
