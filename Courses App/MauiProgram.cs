using Courses_App.Pages;
using Courses_App.Services;
using Microsoft.Extensions.Logging;

namespace Courses_App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
            .UseMauiApp<App>() // Tambahkan baris ini agar aplikasi dapat dijalankan
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

            builder.Services.AddHttpClient<CourseService>(client =>
            {
                client.BaseAddress = new Uri("https://actualbackendapp.azurewebsites.net/api/Courses");
            });

            builder.Services.AddHttpClient<CategoryService>(client =>
            {
                client.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/");
            });

            builder.Services.AddSingleton<CoursePage>();
            builder.Services.AddSingleton<CategoryPage>();

            builder.Services.AddSingleton<CourseService>();
            builder.Services.AddSingleton<CategoryService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
