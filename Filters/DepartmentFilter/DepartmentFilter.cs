namespace MatveevVadimKt_42_22.Filters.DepartmentFilter
{
    public class DepartmentFilter
    {
        public DateTime? FoundedDateFrom { get; set; }
        public DateTime? FoundedDateTo { get; set; }
        public int? MinTeachersCount { get; set; }
        public int? MaxTeachersCount { get; set; }
    }
}
