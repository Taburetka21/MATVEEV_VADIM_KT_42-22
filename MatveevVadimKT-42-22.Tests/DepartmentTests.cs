using MatveevVadimKt_42_22.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatveevVadimKT_42_22.Tests
{
    public class DepartmentTests
    {
        [Fact]
        public void IsValidDepartmentName_ReturnsTrue_ForValidNames()
        {
            var testDepartment = new Department
            {
                Name = "ФУиСТ",
            };

            var result = testDepartment.IsValidDepartmentName();

            Assert.True(result);
        }


        public void IsValidDepartmentDate_ReturnsTrue_WhenDateIsAfter1967()
        {
            // Arrange
            var validDate = new DateTime(1970, 1, 1);
            var department = new Department { FoundedDate = validDate };

            // Act
            var result = department.IsValidDepartmentDate();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidDepartmentDate_ReturnsTrue_WhenDateIsExactly1967()
        {
            // Arrange
            var boundaryDate = new DateTime(1967, 9, 1);
            var department = new Department { FoundedDate = boundaryDate };

            // Act
            var result = department.IsValidDepartmentDate();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidDepartmentDate_ReturnsFalse_WhenDateIsBefore1967()
        {
            // Arrange
            var invalidDate = new DateTime(1960, 1, 1);
            var department = new Department { FoundedDate = invalidDate };

            // Act
            var result = department.IsValidDepartmentDate();

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void IsValidDepartmentHeadId_ReturnTrue_ForValidInt()
        {
            var testDepartment = new Department
            {
                HeadId = 1
            };

            var result = testDepartment.IsValidDepartmentHead();

            Assert.True(result);
        }

        [Fact]
        public void IsValidDegreeInput_ReturnTrue_ForNumericString()
        {
            var result = Department.IsValidNumberInput("123");
            Assert.True(result);
        }

        [Fact]
        public void IsValidDegreeInput_ReturnFalse_ForNonNumericString()
        {
            var result = Department.IsValidNumberInput("abc");
            Assert.False(result);
        }
    }
}
