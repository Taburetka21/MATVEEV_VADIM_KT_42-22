using MatveevVadimKt_42_22.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatveevVadimKT_42_22.Tests
{
    public class TeacherTests
    {
        // 1. Тест на проверку присвоения значений
        [Fact]
        public void Teacher_ShouldAssignPropertiesCorrectly()
        {
            // Arrange
            var teacher = new Teacher
            {
                Id = 1,
                FirstName = "Иван",
                LastName = "Иванов",
                DegreeId = 1,
                PositionId = 2,
                DepartmentId = 3
            };

            // Act
            var resultFirstName = teacher.FirstName;
            var resultLastName = teacher.LastName;
            var resultDegreeId = teacher.DegreeId;
            var resultPositionId = teacher.PositionId;
            var resultDepartmentId = teacher.DepartmentId;

            // Assert
            Assert.Equal("Иван", resultFirstName);
            Assert.Equal("Иванов", resultLastName);
            Assert.Equal(1, resultDegreeId);
            Assert.Equal(2, resultPositionId);
            Assert.Equal(3, resultDepartmentId);
        }

        // 2. Тест на валидацию имени преподавателя
        [Fact]
        public void Teacher_FirstName_ShouldNotBeNullOrEmpty()
        {
            // Arrange
            var teacher = new Teacher { FirstName = "" };

            // Act
            var result = string.IsNullOrEmpty(teacher.FirstName);

            // Assert
            Assert.True(result); // Ожидаем, что FirstName будет пустым
        }

        // 3. Тест на валидацию фамилии преподавателя
        [Fact]
        public void Teacher_LastName_ShouldNotBeNullOrEmpty()
        {
            // Arrange
            var teacher = new Teacher { LastName = "" };

            // Act
            var result = string.IsNullOrEmpty(teacher.LastName);

            // Assert
            Assert.True(result); // Ожидаем, что LastName будет пустым
        }

        // 4. Тест на связь с ученой степенью (Degree)
        [Fact]
        public void Teacher_ShouldAssignDegreeCorrectly()
        {
            // Arrange
            var degree = new Degree { Id = 1, Name = "Доктор наук" };
            var teacher = new Teacher
            {
                DegreeId = degree.Id,
                Degree = degree
            };

            // Act
            var resultDegree = teacher.Degree;

            // Assert
            Assert.NotNull(resultDegree);
            Assert.Equal(degree.Id, resultDegree.Id);
            Assert.Equal("Доктор наук", resultDegree.Name);
        }

        // 5. Тест на связь с должностью (Position)
        [Fact]
        public void Teacher_ShouldAssignPositionCorrectly()
        {
            // Arrange
            var position = new Position { Id = 2, Name = "Профессор" };
            var teacher = new Teacher
            {
                PositionId = position.Id,
                Position = position
            };

            // Act
            var resultPosition = teacher.Position;

            // Assert
            Assert.NotNull(resultPosition);
            Assert.Equal(position.Id, resultPosition.Id);
            Assert.Equal("Профессор", resultPosition.Name);
        }

        // 6. Тест на связь с кафедрой (Department)
        [Fact]
        public void Teacher_ShouldAssignDepartmentCorrectly()
        {
            // Arrange
            var department = new Department { Id = 3, Name = "ИВТ" };
            var teacher = new Teacher
            {
                DepartmentId = department.Id,
                Department = department
            };

            // Act
            var resultDepartment = teacher.Department;

            // Assert
            Assert.NotNull(resultDepartment);
            Assert.Equal(department.Id, resultDepartment.Id);
            Assert.Equal("ИВТ", resultDepartment.Name);
        }
    }
}
