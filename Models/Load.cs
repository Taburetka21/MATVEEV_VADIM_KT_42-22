namespace MatveevVadimKt_42_22.Models
{
    public class Load
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

        public int DisciplineId { get; set; }

        public virtual Discipline Discipline { get; set; }

        public int Hours { get; set; }
        public bool IsValidHours()
        {
            return Hours > 0 && Hours <= 40;// проверка на нормальное количество часов
        }
    }
}
