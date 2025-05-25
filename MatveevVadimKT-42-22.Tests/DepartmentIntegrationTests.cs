using MatveevVadimKt_42_22.Database;
using MatveevVadimKt_42_22.Filters.DepartmentFilter;
using MatveevVadimKt_42_22.Interfaces.DepartmentInterfaces;
using MatveevVadimKt_42_22.Models.DTO;
using MatveevVadimKt_42_22.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatveevVadimKT_42_22.Tests
{
    public class DepartmentIntegrationTests
    {
        private readonly DbContextOptions<UniversityDbContext> _dbContextOptions;

        public DepartmentIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<UniversityDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetDepartmentById_ReturnsCorrectResult()
        {
            var ctx = new UniversityDbContext(_dbContextOptions);
            var departmentService = new DepartmentService(ctx);

            var departments = new List<Department>
            {
                 new Department { Id = 1, Name = "ИВТ", FoundedDate=new DateTime(1980, 9, 1),
                    HeadId = null},
                 new Department { Id = 2, Name = "РЭА", FoundedDate=new DateTime(1990, 9, 1),
                    HeadId = null},
                 new Department { Id = 3, Name = "ИГФ", FoundedDate=new DateTime(2000, 9, 1),
                    HeadId = null},
            };

            await ctx.Set<Department>().AddRangeAsync(departments);
            await ctx.SaveChangesAsync();

            var departmentId = 1;

            var departmentsResult = await departmentService.GetDepartmentByIdAsync(departmentId, CancellationToken.None);

            Assert.NotNull(departmentsResult); // Проверяем, что объект не null
            Assert.Equal(1, departmentsResult.Id); // Проверяем ID
            Assert.Equal("ИВТ", departmentsResult.Name); // Проверяем имя
        }

        [Fact]
        public async Task AddDepartment_WorksCorrectly()
        {
            var ctx = new UniversityDbContext(_dbContextOptions);
            var departmentService = new DepartmentService(ctx);

            var department = new Department
            {
                Id = 1,
                Name = "ИВТ",
                FoundedDate = new DateTime(1980, 9, 1),
                HeadId = null
            };

            var result = await departmentService.AddDepartmentAsync(department, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("ИВТ", result.Name);
        }

        [Fact]
        public async Task UpdateDepartment_WorksCorrectly()
        {
            var ctx = new UniversityDbContext(_dbContextOptions);
            var departmentService = new DepartmentService(ctx);

            var department = new Department
            {
                Id = 1,
                Name = "ИВТ",
                FoundedDate = new DateTime(1980, 9, 1),
                HeadId = null
            };

            await ctx.Departments.AddAsync(department);
            await ctx.SaveChangesAsync();

            var updateDepartment = new UpdateDepartmentDto
            {
                Id = 1,
                Name = "РЭА",
                FoundedDate = new DateTime(1980, 9, 1),
                HeadId = null
            };

            var result = await departmentService.UpdateDepartmentAsync(updateDepartment, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal("РЭА", result.Name);
        }

        [Fact]
        public async Task DeleteDepartment_WorksCorrectly()
        {
            var ctx = new UniversityDbContext(_dbContextOptions);
            var departmentService = new DepartmentService(ctx);

            var department = new Department
            {
                Id = 1,
                Name = "ИВТ",
                FoundedDate = new DateTime(1980, 9, 1),
                HeadId = null
            };

            await ctx.Set<Department>().AddRangeAsync(department);
            await ctx.SaveChangesAsync();

            var result = await departmentService.DeleteDepartmentAsync(1, CancellationToken.None);
            Assert.True(result);

            var deleted = await ctx.Departments.FindAsync(1);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task FilterByDate_ReturnsExpected()
        {
            var ctx = new UniversityDbContext(_dbContextOptions);
            var departmentService = new DepartmentService(ctx);

            var departments = new List<Department>
            {
                 new Department { Id = 1, Name = "ИВТ", FoundedDate=new DateTime(1980, 9, 1),
                    HeadId = 1},
                 new Department { Id = 2, Name = "РЭА", FoundedDate=new DateTime(1990, 9, 1),
                    HeadId = 2},
            };

            var Position = new Position { Id = 1, Name = "Дрцент" };
            await ctx.Positions.AddAsync(Position);

            var Degree = new Degree { Id = 1, Name = "Доктор наук" };
            await ctx.Degrees.AddAsync(Degree);

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
            await ctx.Set<Department>().AddRangeAsync(departments);
            await ctx.SaveChangesAsync();

            var filter = new DepartmentFilter
            {
                FoundedDateFrom = new DateTime(1970, 9, 1),
                FoundedDateTo = new DateTime(1985, 9, 1),
                MinTeachersCount = 2,
                MaxTeachersCount = 5
            };
            var result = await departmentService.GetDepartmentsAsync(filter, CancellationToken.None);

            var departmentList = Assert.IsAssignableFrom<Department[]>(result);
            Assert.Single(departmentList);
        }
    }
}
