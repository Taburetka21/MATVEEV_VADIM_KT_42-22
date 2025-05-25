using MatveevVadimKt_42_22.Database;
using MatveevVadimKt_42_22.Interfaces.TeacherInterfaces;
using MatveevVadimKt_42_22.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatveevVadimKT_42_22.Tests
{
    public class TeacherIntegrationTests
    {
        private readonly DbContextOptions<UniversityDbContext> _dbContextOptions;

        public TeacherIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<UniversityDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Используем временную базу данных
                .Options;
        }

        [Fact]
        public async Task AddTeacher_ShouldReturnTeacher_WhenTeacherIsAdded()
        {
            // Arrange
            var context = new UniversityDbContext(_dbContextOptions);
            var teacherService = new TeacherService(context);

            var newTeacher = new Teacher
            {
                FirstName = "Иван",
                LastName = "Иванов",
                DegreeId = 1,
                PositionId = 1,
                DepartmentId = 1
            };

            // Act
            var result = await teacherService.AddTeacherAsync(newTeacher, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Иван", result.FirstName);
        }

        // 2. Тест на извлечение преподавателя по ID
        [Fact]
        public async Task GetTeacherById_ShouldReturnTeacher_WhenTeacherExists()
        {
            // Arrange
            var context = new UniversityDbContext(_dbContextOptions);
            var teacherService = new TeacherService(context);

            var teacher = new Teacher
            {
                FirstName = "Иван",
                LastName = "Иванов",
                DegreeId = 1,
                PositionId = 1,
                DepartmentId = 1
            };
            await context.AddAsync(teacher);
            await context.SaveChangesAsync();

            // Act
            var result = await teacherService.GetTeacherByIdAsync(teacher.Id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Иван", result.FirstName);
        }

        // 3. Тест на обновление преподавателя
        [Fact]
        public async Task UpdateTeacher_ShouldUpdateTeacher_WhenTeacherExists()
        {
            // Arrange
            var context = new UniversityDbContext(_dbContextOptions);
            var teacherService = new TeacherService(context);

            var teacher = new Teacher
            {
                FirstName = "Иван",
                LastName = "Иванов",
                DegreeId = 1,
                PositionId = 1,
                DepartmentId = 1
            };
            await context.AddAsync(teacher);
            await context.SaveChangesAsync();

            teacher.FirstName = "Петр";  // Обновляем имя

            // Act
            var updatedTeacher = await teacherService.UpdateTeacherAsync(teacher, CancellationToken.None);

            // Assert
            Assert.NotNull(updatedTeacher);
            Assert.Equal("Петр", updatedTeacher.FirstName);
        }

        // 4. Тест на удаление преподавателя
        [Fact]
        public async Task DeleteTeacher_ShouldDeleteTeacher_WhenTeacherExists()
        {
            // Arrange
            var context = new UniversityDbContext(_dbContextOptions);
            var teacherService = new TeacherService(context);

            var teacher = new Teacher
            {
                FirstName = "Иван",
                LastName = "Иванов",
                DegreeId = 1,
                PositionId = 1,
                DepartmentId = 1
            };
            await context.AddAsync(teacher);
            await context.SaveChangesAsync();

            // Act
            var result = await teacherService.DeleteTeacherAsync(teacher.Id, CancellationToken.None);

            // Assert
            Assert.True(result);
            var deletedTeacher = await context.Teachers.FindAsync(teacher.Id);
            Assert.Null(deletedTeacher);
        }
    }
}
