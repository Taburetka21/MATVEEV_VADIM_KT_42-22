using System.Xml.Linq;

namespace MatveevVadimKt_42_22.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime FoundedDate { get; set; }

        public int? HeadId { get; set; }
        public virtual Teacher? Head { get; set; }
        // Метод для проверки валидности имени департамента
        public bool IsValidDepartmentName()
        {
            return !string.IsNullOrEmpty(Name) && Name.Length >= 3;
        }

        // Метод для проверки даты основания департамента (не раньше 1967 года)
        public bool IsValidDepartmentDate()
        {
            return FoundedDate >= new DateTime(1967, 1, 1);
        }

        // Метод для проверки валидности ID руководителя департамента
        public bool IsValidDepartmentHead()
        {
            return HeadId.HasValue && HeadId.Value > 0;
        }

        // Метод для проверки числового ввода (например, для кода или числа)
        public static bool IsValidNumberInput(string input)
        {
            return int.TryParse(input, out _);
        }
    }
    
}
