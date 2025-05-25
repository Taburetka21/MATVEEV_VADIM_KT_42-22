using MatveevVadimKt_42_22.Interfaces.DepartmentInterfaces;

using MatveevVadimKt_42_22.Interfaces.DisciplineInterfaces;
using MatveevVadimKt_42_22.Interfaces.LoadInterfaces;
using MatveevVadimKt_42_22.Interfaces.TeacherInterfaces;
using static MatveevVadimKt_42_22.Interfaces.DisciplineInterfaces.IDisciplineService;

namespace MatveevVadimKt_42_22.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDisciplineService, DisciplineService>();
            services.AddScoped<ILoadService, LoadService>();
            return services;
        }
    }
}
