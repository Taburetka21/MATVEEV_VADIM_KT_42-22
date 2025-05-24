using MatveevVadimKt_42_22.Database;
using MatveevVadimKt_42_22.Filters.TeacherFilter;
using MatveevVadimKt_42_22.Models;
using Microsoft.EntityFrameworkCore;
using MatveevVadimKt_42_22.Models.DTO;


namespace MatveevVadimKt_42_22.Interfaces.TeacherInterfaces
{
    public interface ITeacherService
    {
        public Task<Teacher[]> GetTeachersAsync(TeacherFilter filter, CancellationToken cancellationToken);
        public Task<Teacher?> GetTeacherByIdAsync(int id, CancellationToken cancellationToken);
        public Task<Teacher> AddTeacherAsync(Teacher teacher, CancellationToken cancellationToken);
        public Task<Teacher> UpdateTeacherAsync(Teacher teacher, CancellationToken cancellationToken);
        public Task<bool> DeleteTeacherAsync(int id, CancellationToken cancellationToken);
    }

    public class TeacherService : ITeacherService
    {
        private readonly UniversityDbContext _dbContext;
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

            // Фильтрация по ученой степени
            if (!string.IsNullOrEmpty(filter.Degree))
            {
                query = query.Where(t => t.Degree.Name.Contains(filter.Degree));
            }

            // Фильтрация по должности
            if (!string.IsNullOrEmpty(filter.Position))
            {
                query = query.Where(t => t.Position.Name.Contains(filter.Position));
            }

            // Фильтрация по кафедре
            if (!string.IsNullOrEmpty(filter.Department))
            {
                query = query.Where(t => t.Department.Name.Contains(filter.Department));
            }

            return await query.ToArrayAsync(cancellationToken);
        }

        public async Task<Teacher?> GetTeacherByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Teachers
                .Include(t => t.Degree)
                .Include(t => t.Position)
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<Teacher> AddTeacherAsync(Teacher teacher, CancellationToken cancellationToken)
        {
            _dbContext.Teachers.Add(teacher);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return teacher;
        }

        public async Task<Teacher> UpdateTeacherAsync(Teacher teacher, CancellationToken cancellationToken)
        {
            _dbContext.Teachers.Update(teacher);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return teacher;
        }

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

