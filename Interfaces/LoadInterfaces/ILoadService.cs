using MatveevVadimKt_42_22.Database;
using MatveevVadimKt_42_22.Filters.LoadFilter;
using MatveevVadimKt_42_22.Models;
using Microsoft.EntityFrameworkCore;
using MatveevVadimKt_42_22.Models.DTO;


namespace MatveevVadimKt_42_22.Interfaces.LoadInterfaces
{
    public interface ILoadService
    {
        // Получение списка нагрузок с фильтрацией
        Task<LoadDto[]> GetLoadsAsync(LoadFilter filter, CancellationToken cancellationToken);

        // Получение нагрузки по ID
        Task<LoadDto?> GetLoadByIdAsync(int id, CancellationToken cancellationToken);

        // Добавление новой нагрузки
        Task<LoadDto> AddLoadAsync(AddLoadDto loadDto, CancellationToken cancellationToken);

        // Обновление существующей нагрузки
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
                .AsNoTracking()
                .Include(l => l.Teacher)
                .Include(l => l.Discipline)
                .Include(l => l.Teacher.Department)
                .AsQueryable();

            // Применяем фильтрацию по каждому полю
            if (filter.TeacherId.HasValue)
                query = query.Where(l => l.TeacherId == filter.TeacherId.Value);

            if (filter.DepartmentId.HasValue)
                query = query.Where(l => l.Teacher.DepartmentId == filter.DepartmentId.Value);

            if (filter.DisciplineId.HasValue)
                query = query.Where(l => l.DisciplineId == filter.DisciplineId.Value);

            if (filter.MinHours.HasValue)
                query = query.Where(l => l.Hours >= filter.MinHours.Value);

            if (filter.MaxHours.HasValue)
                query = query.Where(l => l.Hours <= filter.MaxHours.Value);

            return await query
                .Select(l => new LoadDto
                {
                    Id = l.Id,
                    TeacherId = l.TeacherId,
                    DisciplineId = l.DisciplineId,
                    Hours = l.Hours
                })
                .ToArrayAsync(cancellationToken);
        }

        public async Task<LoadDto?> GetLoadByIdAsync(int id, CancellationToken cancellationToken)
        {
            var load = await _dbContext.Loads
                .Where(l => l.Id == id)
                .Select(l => new LoadDto
                {
                    Id = l.Id,
                    TeacherId = l.TeacherId,
                    DisciplineId = l.DisciplineId,
                    Hours = l.Hours
                })
                .FirstOrDefaultAsync(cancellationToken);

            return load;
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

            return new LoadDto
            {
                Id = load.Id,
                TeacherId = load.TeacherId,
                DisciplineId = load.DisciplineId,
                Hours = load.Hours
            };
        }

        public async Task<LoadDto> UpdateLoadAsync(UpdateLoadDto loadDto, CancellationToken cancellationToken)
        {
            var load = await _dbContext.Loads.FindAsync(new object[] { loadDto.Id }, cancellationToken);
            if (load == null)
            {
                return null;
            }

            load.TeacherId = loadDto.TeacherId;
            load.DisciplineId = loadDto.DisciplineId;
            load.Hours = loadDto.Hours;

            _dbContext.Loads.Update(load);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new LoadDto
            {
                Id = load.Id,
                TeacherId = load.TeacherId,
                DisciplineId = load.DisciplineId,
                Hours = load.Hours
            };
        }

        // Добавляем метод для удаления нагрузки
        public async Task<bool> DeleteLoadAsync(int id, CancellationToken cancellationToken)
        {
            var load = await _dbContext.Loads.FindAsync(new object[] { id }, cancellationToken);
            if (load == null)
            {
                return false;  // Если нагрузка не найдена, возвращаем false
            }

            _dbContext.Loads.Remove(load);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;  // Возвращаем true, если нагрузка была удалена
        }
    }

}
