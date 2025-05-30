namespace MatveevVadimKt_42_22.Models
{
    public class Discipline
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        // Метод для проверки валидности имени дисциплины
        public bool IsValidDisciplineName()
        {
            // Имя не может быть пустым или слишком коротким
            return !string.IsNullOrEmpty(Name) && Name.Length >= 3;
        }

        // Метод для валидации дисциплины
        public bool IsValid()
        {
            // Валидация имени дисциплины
            return IsValidDisciplineName();
        }

        // Метод для получения более подробной ошибки (если необходимо для сложной логики)
        public List<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(Name))
                errors.Add("Требуется имя");
            if (Name.Length < 3)
                errors.Add("Имя должно быть минимум 3 символа");

            return errors;
        }
    }

}
