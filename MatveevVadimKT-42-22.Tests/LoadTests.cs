using MatveevVadimKt_42_22.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatveevVadimKT_42_22.Tests
{
    public class LoadTests
    {
        // 1. Тест на проверку присвоения значений
        [Fact]
        public void Load_ShouldAssignPropertiesCorrectly()
        {
            // Arrange
            var load = new Load
            {
                Id = 1,
                TeacherId = 1,
                DisciplineId = 1,
                Hours = 20
            };

            // Act
            var resultTeacherId = load.TeacherId;
            var resultDisciplineId = load.DisciplineId;
            var resultHours = load.Hours;

            // Assert
            Assert.Equal(1, resultTeacherId);
            Assert.Equal(1, resultDisciplineId);
            Assert.Equal(20, resultHours);
        }

        // 2. Тест на валидацию количества часов
        [Fact]
        public void IsValidHours_ShouldReturnTrue_WhenHoursAreValid()
        {
            // Arrange
            var load = new Load { Hours = 20 };

            // Act
            var result = load.IsValidHours();

            // Assert
            Assert.True(result);  // Ожидаем, что количество часов будет валидным
        }

        [Fact]
        public void IsValidHours_ShouldReturnFalse_WhenHoursAreZero()
        {
            // Arrange
            var load = new Load { Hours = 0 };

            // Act
            var result = load.IsValidHours();

            // Assert
            Assert.False(result);  // Ожидаем, что метод вернет false, так как 0 часов - это невалидное значение
        }

        [Fact]
        public void IsValidHours_ShouldReturnFalse_WhenHoursAreTooHigh()
        {
            // Arrange
            var load = new Load { Hours = 50 };

            // Act
            var result = load.IsValidHours();

            // Assert
            Assert.False(result);  // Ожидаем, что метод вернет false, так как 50 часов - это невалидное значение
        }

        // 3. Тест на связь с преподавателем
        [Fact]
        public void Load_ShouldAssignTeacherCorrectly()
        {
            // Arrange
            var teacher = new Teacher { Id = 1, FirstName = "Иван", LastName = "Иванов" };
            var load = new Load
            {
                TeacherId = teacher.Id,
                Teacher = teacher
            };

            // Act
            var resultTeacher = load.Teacher;

            // Assert
            Assert.NotNull(resultTeacher);
            Assert.Equal(teacher.Id, resultTeacher.Id);
            Assert.Equal("Иван", resultTeacher.FirstName);
            Assert.Equal("Иванов", resultTeacher.LastName);
        }

        // 4. Тест на связь с дисциплиной
        [Fact]
        public void Load_ShouldAssignDisciplineCorrectly()
        {
            // Arrange
            var discipline = new Discipline { Id = 1, Name = "Математика" };
            var load = new Load
            {
                DisciplineId = discipline.Id,
                Discipline = discipline
            };

            // Act
            var resultDiscipline = load.Discipline;

            // Assert
            Assert.NotNull(resultDiscipline);
            Assert.Equal(discipline.Id, resultDiscipline.Id);
            Assert.Equal("Математика", resultDiscipline.Name);
        }
    }
}
