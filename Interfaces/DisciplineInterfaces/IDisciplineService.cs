using MatveevVadimKt_42_22.Database;
using MatveevVadimKt_42_22.Filters.DisciplineFilter;
using MatveevVadimKt_42_22.Models;
using Microsoft.EntityFrameworkCore;
using MatveevVadimKt_42_22.Models.DTO;


namespace MatveevVadimKt_42_22.Interfaces.DisciplineInterfaces
{
    public interface IDisciplineService
    {
        Task<Discipline[]> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken cancellationToken);
        Task<Discipline?> GetDisciplineByIdAsync(int id, CancellationToken cancellationToken);
        Task<Discipline> AddDisciplineAsync(Discipline discipline, CancellationToken cancellationToken);
        Task<Discipline> UpdateDisciplineAsync(Discipline discipline, CancellationToken cancellationToken);
        Task<bool> DeleteDisciplineAsync(int id, CancellationToken cancellationToken);
    }

    public class DisciplineService : IDisciplineService
    {
        private readonly UniversityDbContext _dbContext;

        public DisciplineService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Discipline[]> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken cancellationToken)
        {
            var query = _dbContext.Disciplines
                .AsNoTracking()
                .AsQueryable();

            // Фильтрация по преподавателю
            if (filter.TeacherId.HasValue)
            {
                query = query.Where(d => _dbContext.Loads
                    .Any(l => l.DisciplineId == d.Id && l.TeacherId == filter.TeacherId));
            }

            // Фильтрация по нагрузке
            if (filter.MinHours.HasValue || filter.MaxHours.HasValue)
            {
                var disciplineHours = _dbContext.Loads
                    .GroupBy(l => l.DisciplineId)
                    .Select(g => new
                    {
                        DisciplineId = g.Key,
                        TotalHours = g.Sum(l => l.Hours)
                    });

                if (filter.MinHours.HasValue)
                    disciplineHours = disciplineHours.Where(x => x.TotalHours >= filter.MinHours.Value);

                if (filter.MaxHours.HasValue)
                    disciplineHours = disciplineHours.Where(x => x.TotalHours <= filter.MaxHours.Value);

                var disciplineIds = await disciplineHours
                    .Select(x => x.DisciplineId)
                    .ToListAsync(cancellationToken);

                query = query.Where(d => disciplineIds.Contains(d.Id));
            }

            return await query.ToArrayAsync(cancellationToken);
        }

        public async Task<Discipline?> GetDisciplineByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Disciplines
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<Discipline> AddDisciplineAsync(Discipline discipline, CancellationToken cancellationToken)
        {
            _dbContext.Disciplines.Add(discipline);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return discipline;
        }

        public async Task<Discipline> UpdateDisciplineAsync(Discipline discipline, CancellationToken cancellationToken)
        {
            _dbContext.Disciplines.Update(discipline);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return discipline;
        }

        public async Task<bool> DeleteDisciplineAsync(int id, CancellationToken cancellationToken)
        {
            // Удаление связанных нагрузок
            var loadsToDelete = await _dbContext.Loads
                .Where(l => l.DisciplineId == id)
                .ToListAsync(cancellationToken);

            if (loadsToDelete.Any())
            {
                _dbContext.Loads.RemoveRange(loadsToDelete);
            }

            var discipline = await _dbContext.Disciplines
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

            if (discipline == null) return false;

            _dbContext.Disciplines.Remove(discipline);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
