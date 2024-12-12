using Courses_App.Models;
using Courses_App.Services;

namespace Courses_App.Pages;

public partial class AddEnrollment : ContentPage
{
    private readonly CourseService _courseService;
    private List<Course> _courses = new List<Course>();
    private readonly InstructorService _instructorService;
    private List<Instructor> _instructors = new List<Instructor>();
    private readonly EnrollmentService _enrollmentService;
    public AddEnrollment()
    {
        InitializeComponent();
        _courseService = new CourseService(new HttpClient());
        _instructorService = new InstructorService(new HttpClient());
        _enrollmentService = new EnrollmentService(new HttpClient());
        LoadPicker();
    }

    private async void LoadPicker()
    {
        _courses = (await _courseService.GetAllCoursesAsync()).ToList();
        CoursePicker.ItemsSource = _courses;
        CoursePicker.ItemDisplayBinding = new Binding("Name");

        _instructors = (await _instructorService.GetAllInstructorsAsync()).ToList();
        InstructorPicker.ItemsSource = _instructors;
        InstructorPicker.ItemDisplayBinding = new Binding("fullName");
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var selectedInstructor = (Instructor)InstructorPicker.SelectedItem;
        var selectedCourse = (Course)CoursePicker.SelectedItem;
        if (selectedInstructor != null && selectedCourse != null)
        {
            var enrollment = new Enrollment
            {
                instructorId = selectedInstructor.instructorId,
                courseId = selectedCourse.CourseId
            };
            // Send enrollment to API
            try
            {
                await _enrollmentService.CreateEnrollmentAsync(enrollment);
                await DisplayAlert("Success", $"Instructor: {selectedInstructor.fullName}, Course: {selectedCourse.Name}", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Please select both an instructor and a course.", "OK");
        }
    }
}
