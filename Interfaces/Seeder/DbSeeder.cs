using MatveevVadimKt_42_22.Models;
using MatveevVadimKt_42_22.Database;

namespace MatveevVadimKt_42_22.Interfaces.Seeder
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UniversityDbContext>();

            await context.Database.EnsureCreatedAsync();

            if (!context.Departments.Any())
            {
                // 1. Ученые степени и должности
                var degree1 = new Degree { Name = "Кандидат наук" };
                var degree2 = new Degree { Name = "Доктор наук" };

                var position1 = new Position { Name = "Доцент" };
                var position2 = new Position { Name = "Профессор" };

                await context.Degrees.AddRangeAsync(degree1, degree2);
                await context.Positions.AddRangeAsync(position1, position2);
                await context.SaveChangesAsync();

                // 2. Кафедры (без заведующих)
                var kaf1 = new Department
                {
                    Name = "kaf1",
                    FoundedDate = DateTime.Now
                    // HeadId будет добавлен позже
                };

                var kaf2 = new Department
                {
                    Name = "kaf2",
                    FoundedDate = DateTime.Now
                };

                await context.Departments.AddRangeAsync(kaf1, kaf2);
                await context.SaveChangesAsync();

                // 3. Преподаватели (уже с заданным DepartmentId)
                var ivanov = new Teacher
                {
                    FirstName = "Иван",
                    LastName = "Иванов",
                    DegreeId = degree1.Id,
                    PositionId = position1.Id,
                    DepartmentId = kaf1.Id
                };

                var petrov = new Teacher
                {
                    FirstName = "Петр",
                    LastName = "Петров",
                    DegreeId = degree2.Id,
                    PositionId = position2.Id,
                    DepartmentId = kaf2.Id
                };

                var sidorov = new Teacher
                {
                    FirstName = "Сидор",
                    LastName = "Сидоров",
                    DegreeId = degree1.Id,
                    PositionId = position2.Id,
                    DepartmentId = kaf2.Id
                };

                await context.Teachers.AddRangeAsync(ivanov, petrov, sidorov);
                await context.SaveChangesAsync();

                // 4. Назначаем заведующих кафедрами
                kaf1.HeadId = ivanov.Id;
                kaf2.HeadId = petrov.Id;

                context.Departments.UpdateRange(kaf1, kaf2);
                await context.SaveChangesAsync();

                // 5. Дисциплины
                var d1 = new Discipline { Name = "d1", DepartmentId = kaf2.Id };
                var d2 = new Discipline { Name = "d2", DepartmentId = kaf1.Id };

                await context.Disciplines.AddRangeAsync(d1, d2);
                await context.SaveChangesAsync();

                // 6. Нагрузка
                var load1 = new Load { DisciplineId = d1.Id, TeacherId = petrov.Id,Hours = 10};
                var load2 = new Load { DisciplineId = d1.Id, TeacherId = sidorov.Id, Hours = 20 };
                var load3 = new Load { DisciplineId = d2.Id, TeacherId = ivanov.Id, Hours = 30 };
                var load4 = new Load { DisciplineId = d2.Id, TeacherId = sidorov.Id, Hours = 40 };

                await context.Loads.AddRangeAsync(load1, load2, load3, load4);
                await context.SaveChangesAsync();
            }
        }
    }
}
