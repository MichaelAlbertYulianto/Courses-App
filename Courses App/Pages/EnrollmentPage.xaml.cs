using Courses_App.Models;
using Courses_App.Services;
using System.Collections.ObjectModel;

namespace Courses_App.Pages;

public partial class EnrollmentPage : ContentPage
{
    private readonly EnrollmentService _enrollmentService;
    private readonly InstructorService _instructorService;
    private readonly CourseService _courseService;
    private ObservableCollection<EnrollmentWithSelected> _enrollment;
    public EnrollmentPage(EnrollmentService enrollmentService, InstructorService instructorService, CourseService courseService)
    {
        InitializeComponent();
        _enrollmentService = enrollmentService;
        _instructorService = instructorService;
        _courseService = courseService;
        _enrollment = new ObservableCollection<EnrollmentWithSelected>();
        EnrollmentListView.ItemsSource = _enrollment;

        // Use async void to avoid CS4014 warning
        LoadEnrollments();
    }


    private async void LoadEnrollments()
    {
        try
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            _enrollment.Clear();

            foreach (var enrollment in enrollments)
            {
                _enrollment.Add(new EnrollmentWithSelected
                {
                    enrollmentId = enrollment.enrollmentId,
                    instructorId = enrollment.instructorId,
                    courseId = enrollment.courseId,
                    enrolledAt = enrollment.enrolledAt,
                });
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load enrollments: {ex.Message}", "OK");
        }
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        LoadEnrollments();
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddEnrollment());
    }
    public EnrollmentPage()
    {
        InitializeComponent();
        _enrollmentService = new EnrollmentService(new HttpClient());
        _enrollment = new ObservableCollection<EnrollmentWithSelected>();
        EnrollmentListView.ItemsSource = _enrollment;

        // Use async void to avoid CS4014 warning
        LoadEnrollments();
    }
}
