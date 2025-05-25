using MatveevVadimKt_42_22.Database;
using MatveevVadimKt_42_22.Filters.DisciplineFilter;
using MatveevVadimKt_42_22.Interfaces.DisciplineInterfaces;
using MatveevVadimKt_42_22.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatveevVadimKT_42_22.Tests
{
    public class DisciplineIntegrationTests
    {
        private readonly DbContextOptions<UniversityDbContext> _dbContextOptions;

        public DisciplineIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<UniversityDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetDisciplineById_ReturnsCorrectResult()
        {
            var ctx = new UniversityDbContext(_dbContextOptions);
            var disciplineService = new DisciplineService(ctx);

            var disciplines = new List<Discipline>
            {
                 new Discipline { Id = 1, Name = "Русский"},
                 new Discipline { Id = 2, Name = "Математика"},
                 new Discipline { Id = 3, Name = "Физика"}
            };

            await ctx.Set<Discipline>().AddRangeAsync(disciplines);
            await ctx.SaveChangesAsync();

            var disId = 2;

            var disResult = await disciplineService.GetDisciplineByIdAsync(disId, CancellationToken.None);

            Assert.NotNull(disResult); // Проверяем, что объект не null
            Assert.Equal(2, disResult.Id); // Проверяем ID
            Assert.Equal("Математика", disResult.Name); // Проверяем имя
        }

        [Fact]
        public async Task AddDiscipline_WorksCorrectly()
        {
            var ctx = new UniversityDbContext(_dbContextOptions);
            var disService = new DisciplineService(ctx);

            var dis = new Discipline
            {
                Id = 1,
                Name = "Математика"
            };

            var result = await disService.AddDisciplineAsync(dis, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Математика", result.Name);
        }

        [Fact]
        public async Task UpdateDiscipline_WorksCorrectly()
        {
            var ctx = new UniversityDbContext(_dbContextOptions);
            var disService = new DisciplineService(ctx);

            var dis = new Discipline
            {
                Id = 1,
                Name = "Старый",
            };

            await ctx.Disciplines.AddAsync(dis);
            await ctx.SaveChangesAsync();

            var updatedDis = new Discipline
            {
                Id = 1, // Тот же ID, что и у originalTeacher
                Name = "Обновлённый"
            };

            // Act
            var result = await disService.UpdateDisciplineAsync(updatedDis, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Обновлённый", result.Name);
        }

        [Fact]
        public async Task DeleteTeacher_WorksCorrectly()
        {
            var ctx = new UniversityDbContext(_dbContextOptions);
            var disService = new DisciplineService(ctx);

            var dis = new Discipline
            {
                Id = 1,
                Name = "Старый",
            };

            await ctx.Disciplines.AddAsync(dis);
            await ctx.SaveChangesAsync();

            var result = await disService.DeleteDisciplineAsync(1, CancellationToken.None);
            Assert.True(result);

            var deleted = await ctx.Teachers.FindAsync(1);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task FilterByteachers_ReturnsExpected()
        {
            var ctx = new UniversityDbContext(_dbContextOptions);
            var disService = new DisciplineService(ctx);

            var Departments = new List<Department> {
                new Department { Id = 1, Name = "ИВТ" },
                    new Department { Id = 2, Name = "РЭА" }
            };
            await ctx.Departments.AddRangeAsync(Departments);

            var ivtPosition = new Position { Id = 1, Name = "Доцент" };
            await ctx.Positions.AddAsync(ivtPosition);

            var ivtDegree = new Degree { Id = 1, Name = "Доктор наук" };
            await ctx.Degrees.AddAsync(ivtDegree);

            var teachers = new List<Teacher>
            {
                 new Teacher { Id = 1, FirstName = "Иван", LastName = "Иванов",
                    DegreeId = 1, PositionId = 1, DepartmentId = 2 },
                 new Teacher { Id = 2, FirstName = "Петр", LastName = "Петров",
                    DegreeId = 1, PositionId = 1, DepartmentId = 1 },
                 new Teacher { Id = 3, FirstName = "Сергей", LastName = "Сергеев",
                    DegreeId = 1, PositionId = 1, DepartmentId = 1 }
            };

            await ctx.Set<Teacher>().AddRangeAsync(teachers);

            var dis = new List<Discipline>
            {
                new Discipline { Id = 1, Name = "Проектный практикум"},
                new Discipline { Id = 2, Name= "Психология"}
            };

            await ctx.Set<Discipline>().AddRangeAsync(dis);

            var loads = new List<Load>
            {
                new Load { Id = 1, DisciplineId = 1, TeacherId = 1, Hours = 20},
                new Load { Id = 2, DisciplineId = 1, TeacherId = 2, Hours = 30},
                new Load { Id = 3, DisciplineId = 2, TeacherId = 2, Hours = 50},
                new Load { Id = 4, DisciplineId = 2, TeacherId = 3, Hours = 10}
            };
            await ctx.Set<Load>().AddRangeAsync(loads);
            await ctx.SaveChangesAsync();

            var filter = new DisciplineFilter { TeacherId = null, MinHours = 25, MaxHours = 60 }; ;
            var result = await disService.GetDisciplinesAsync(filter, CancellationToken.None);

            var disList = Assert.IsAssignableFrom<Discipline[]>(result);
            Assert.Equal(2, disList.Count());
        }

    }
}
