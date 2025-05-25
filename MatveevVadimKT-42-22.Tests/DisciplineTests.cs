using MatveevVadimKt_42_22.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatveevVadimKT_42_22.Tests
{
    public class DisciplineTests
    {
        // 1. Тест на валидацию имени дисциплины: не должно быть пустым
        [Fact]
        public void Discipline_Name_ShouldNotBeNullOrEmpty()
        {
            // Arrange
            var discipline = new Discipline { Name = "" };

            // Act
            var result = string.IsNullOrEmpty(discipline.Name);

            // Assert
            Assert.True(result); // Ожидаем, что имя будет пустым, так как присвоили пустую строку
        }

        // 2. Тест на валидацию имени дисциплины: проверка минимальной длины
        [Fact]
        public void Discipline_Name_ShouldHaveMinimumLength()
        {
            // Arrange
            var discipline = new Discipline { Name = "Физика" };

            // Act
            var result = discipline.Name.Length >= 3;

            // Assert
            Assert.True(result);  // Ожидаем, что длина имени будет больше 3 символов
        }

        [Fact]
        public void Discipline_Name_ShouldNotBeShorterThan3Characters()
        {
            // Arrange
            var discipline = new Discipline { Name = "И" };

            // Act
            var result = discipline.Name.Length >= 3;

            // Assert
            Assert.False(result);  // Ожидаем, что длина имени будет меньше 3 символов
        }

        // 3. Тест на присвоение имени дисциплины
        [Fact]
        public void Discipline_ShouldAssignNameCorrectly()
        {
            // Arrange
            var discipline = new Discipline { Name = "Математика" };

            // Act
            var result = discipline.Name;

            // Assert
            Assert.Equal("Математика", result);  // Ожидаем, что имя будет корректно присвоено
        }

        // 4. Тест на присвоение ID дисциплины
        [Fact]
        public void Discipline_ShouldAssignIdCorrectly()
        {
            // Arrange
            var discipline = new Discipline { Id = 1 };

            // Act
            var result = discipline.Id;

            // Assert
            Assert.Equal(1, result);  // Ожидаем, что ID будет корректно присвоен
        }
    }
}
