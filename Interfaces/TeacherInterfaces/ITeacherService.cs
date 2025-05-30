using MatveevVadimKt_42_22.Database;
using MatveevVadimKt_42_22.Filters.TeacherFilter;
using MatveevVadimKt_42_22.Models;
using Microsoft.EntityFrameworkCore;
using MatveevVadimKt_42_22.Models.DTO;


namespace MatveevVadimKt_42_22.Interfaces.TeacherInterfaces
{
    public interface ITeacherService
    {
        Task<Teacher[]> GetTeachersAsync(TeacherFilter filter, CancellationToken cancellationToken);
        Task<Teacher?> GetTeacherByIdAsync(int id, CancellationToken cancellationToken);
        Task<Teacher> AddTeacherAsync(Teacher teacher, CancellationToken cancellationToken);
        Task<Teacher> UpdateTeacherAsync(Teacher teacher, CancellationToken cancellationToken);
        Task<bool> DeleteTeacherAsync(int id, CancellationToken cancellationToken);
    }

    public class TeacherService : ITeacherService
    {
        private readonly UniversityDbContext _dbContext;

        // Конструктор, принимающий DbContext
        public TeacherService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Teacher[]> GetTeachersAsync(TeacherFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Teachers
                .AsNoTracking()
                .Include(t => t.Degree)
                .Include(t => t.Position)
                .Include(t => t.Department)
                .AsQueryable();

            // Фильтрация по DegreeId, если задано
            if (filter.DegreeId.HasValue && filter.DegreeId.Value != 0)
            {
                query = query.Where(t => t.DegreeId == filter.DegreeId.Value);
            }

            // Фильтрация по PositionId, если задано
            if (filter.PositionId.HasValue && filter.PositionId.Value != 0)
            {
                query = query.Where(t => t.PositionId == filter.PositionId.Value);
            }

            // Фильтрация по DepartmentId, если задано
            if (filter.DepartmentId.HasValue && filter.DepartmentId.Value != 0)
            {
                query = query.Where(t => t.DepartmentId == filter.DepartmentId.Value);
            }

            return await query.ToArrayAsync(cancellationToken);
        }


        // Получение преподавателя по ID
        public async Task<Teacher?> GetTeacherByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Teachers
                .Include(t => t.Degree)
                .Include(t => t.Position)
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        // Добавление нового преподавателя
        public async Task<Teacher> AddTeacherAsync(Teacher teacher, CancellationToken cancellationToken)
        {
            _dbContext.Teachers.Add(teacher);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return teacher;
        }

        // Обновление данных преподавателя
        public async Task<Teacher> UpdateTeacherAsync(Teacher teacher, CancellationToken cancellationToken)
        {
            _dbContext.Teachers.Update(teacher);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return teacher;
        }

        // Удаление преподавателя по ID
        public async Task<bool> DeleteTeacherAsync(int id, CancellationToken cancellationToken)
        {
            var teacher = await _dbContext.Teachers.FindAsync(new object[] { id }, cancellationToken);
            if (teacher == null) return false;

            _dbContext.Teachers.Remove(teacher);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

