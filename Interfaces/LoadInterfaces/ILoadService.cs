using MatveevVadimKt_42_22.Database;
using MatveevVadimKt_42_22.Filters.LoadFilter;
using MatveevVadimKt_42_22.Models;
using Microsoft.EntityFrameworkCore;
using MatveevVadimKt_42_22.Models.DTO;


namespace MatveevVadimKt_42_22.Interfaces.LoadInterfaces
{
    public interface ILoadService
    {
        Task<LoadDto[]> GetLoadsAsync(LoadFilter filter, CancellationToken cancellationToken);
        Task<LoadDto?> GetLoadByIdAsync(int id, CancellationToken cancellationToken);
        Task<LoadDto> AddLoadAsync(AddLoadDto loadDto, CancellationToken cancellationToken);
        Task<LoadDto> UpdateLoadAsync(UpdateLoadDto loadDto, CancellationToken cancellationToken);
    }

    public class LoadService : ILoadService
    {
        private readonly UniversityDbContext _dbContext;

        public LoadService(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LoadDto[]> GetLoadsAsync(LoadFilter filter, CancellationToken cancellationToken)
        {
            var query = _dbContext.Loads
                .Include(l => l.Teacher)
                .ThenInclude(t => t.Department)
                .Include(l => l.Discipline)
                .AsNoTracking()
                .AsQueryable();

            // Применяем фильтры
            if (filter.TeacherId.HasValue)
            {
                query = query.Where(l => l.TeacherId == filter.TeacherId.Value);
            }

            if (filter.DepartmentId.HasValue)
            {
                query = query.Where(l => l.Teacher.DepartmentId == filter.DepartmentId.Value);
            }

            if (filter.DisciplineId.HasValue)
            {
                query = query.Where(l => l.DisciplineId == filter.DisciplineId.Value);
            }

            if (filter.MinHours.HasValue)
            {
                query = query.Where(l => l.Hours >= filter.MinHours.Value);
            }

            if (filter.MaxHours.HasValue)
            {
                query = query.Where(l => l.Hours <= filter.MaxHours.Value);
            }

            return await query.Select(l => new LoadDto
            {
                Id = l.Id,
                TeacherId = l.TeacherId,
                TeacherName = $"{l.Teacher.LastName} {l.Teacher.FirstName}",
                DepartmentId = l.Teacher.DepartmentId,
                DepartmentName = l.Teacher.Department.Name,
                DisciplineId = l.DisciplineId,
                DisciplineName = l.Discipline.Name,
                Hours = l.Hours
            }).ToArrayAsync(cancellationToken);
        }

        public async Task<LoadDto?> GetLoadByIdAsync(int id, CancellationToken cancellationToken)
        {
            var load = await _dbContext.Loads
                .Include(l => l.Teacher)
                .ThenInclude(t => t.Department)
                .Include(l => l.Discipline)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

            if (load == null) return null;

            return new LoadDto
            {
                Id = load.Id,
                TeacherId = load.TeacherId,
                TeacherName = $"{load.Teacher.LastName} {load.Teacher.FirstName}",
                DepartmentId = load.Teacher.DepartmentId,
                DepartmentName = load.Teacher.Department.Name,
                DisciplineId = load.DisciplineId,
                DisciplineName = load.Discipline.Name,
                Hours = load.Hours
            };
        }

        public async Task<LoadDto> AddLoadAsync(AddLoadDto loadDto, CancellationToken cancellationToken)
        {
            var load = new Load
            {
                TeacherId = loadDto.TeacherId,
                DisciplineId = loadDto.DisciplineId,
                Hours = loadDto.Hours
            };

            _dbContext.Loads.Add(load);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return await GetLoadByIdAsync(load.Id, cancellationToken);
        }

        public async Task<LoadDto> UpdateLoadAsync(UpdateLoadDto loadDto, CancellationToken cancellationToken)
        {
            var load = await _dbContext.Loads
                .FirstOrDefaultAsync(l => l.Id == loadDto.Id, cancellationToken);

            if (load == null) throw new Exception("Нагрузка не найдена");

            load.TeacherId = loadDto.TeacherId;
            load.DisciplineId = loadDto.DisciplineId;
            load.Hours = loadDto.Hours;

            _dbContext.Loads.Update(load);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return await GetLoadByIdAsync(load.Id, cancellationToken);
        }
    }
}
