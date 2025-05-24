using System.ComponentModel.DataAnnotations;

namespace MatveevVadimKt_42_22.Models.DTO
{
    public class LoadDto
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DisciplineId { get; set; }
        public string DisciplineName { get; set; }
        public int Hours { get; set; }
    }

    public class AddLoadDto
    {
        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int DisciplineId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Hours { get; set; }
    }

    public class UpdateLoadDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int DisciplineId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Hours { get; set; }
    }
}
