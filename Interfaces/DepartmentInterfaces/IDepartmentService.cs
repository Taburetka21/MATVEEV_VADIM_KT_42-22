using MatveevVadimKt_42_22.Database;
using MatveevVadimKt_42_22.Filters.DepartmentFilter;
using MatveevVadimKt_42_22.Models;
using Microsoft.EntityFrameworkCore;
using MatveevVadimKt_42_22.Models.DTO;

namespace MatveevVadimKt_42_22.Interfaces.DepartmentInterfaces
{
    public interface IDepartmentService
    {
        Task<Department[]> GetDepartmentsAsync(DepartmentFilter filter, CancellationToken cancellationToken);
        Task<Department?> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken);
        Task<Department> AddDepartmentAsync(Department department, CancellationToken cancellationToken);
        Task<Department> UpdateDepartmentAsync(UpdateDepartmentDto departmentDto, CancellationToken cancellationToken);
        Task<bool> DeleteDepartmentAsync(int id, CancellationToken cancellationToken);
    }

    namespace Krylov_KT_42_22.Interfaces.DepartmentInterfaces
    {
        public class DepartmentService : IDepartmentService
        {
            private readonly UniversityDbContext _dbContext;

            public DepartmentService(UniversityDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Department[]> GetDepartmentsAsync(DepartmentFilter filter, CancellationToken cancellationToken)
            {
                var query = _dbContext.Departments
                    .AsNoTracking()
                    .Include(d => d.Head)
                    .AsQueryable();

                if (filter != null)
                {
                    if (filter.FoundedDateFrom.HasValue)
                        query = query.Where(d => d.FoundedDate >= filter.FoundedDateFrom.Value);

                    if (filter.FoundedDateTo.HasValue)
                        query = query.Where(d => d.FoundedDate <= filter.FoundedDateTo.Value);

                    if (filter.MinTeachersCount.HasValue || filter.MaxTeachersCount.HasValue)
                    {
                        // Измененный подход для включения факультетов без преподавателей
                        var departmentCounts = await _dbContext.Teachers
                            .GroupBy(t => t.DepartmentId)
                            .Select(g => new
                            {
                                DepartmentId = g.Key,
                                Count = g.Count()
                            })
                            .ToListAsync(cancellationToken);

                        // Если указан MinTeachersCount = 0, включаем факультеты без преподавателей
                        if (filter.MinTeachersCount.HasValue && filter.MinTeachersCount.Value == 0)
                        {
                            var departmentsWithTeachers = departmentCounts
                                .Where(x => x.Count >= filter.MinTeachersCount.Value)
                                .Select(x => x.DepartmentId)
                                .ToList();

                            if (filter.MaxTeachersCount.HasValue)
                            {
                                departmentsWithTeachers = departmentCounts
                                    .Where(x => x.Count <= filter.MaxTeachersCount.Value)
                                    .Select(x => x.DepartmentId)
                                    .ToList();
                            }

                            // Включаем факультеты без преподавателей (тех, кого нет в departmentCounts)
                            var allDepartmentIds = await _dbContext.Departments
                                .Select(d => d.Id)
                                .ToListAsync(cancellationToken);

                            var departmentsWithoutTeachers = allDepartmentIds
                                .Except(departmentCounts.Select(x => x.DepartmentId));

                            var finalDepartmentIds = departmentsWithTeachers.Union(departmentsWithoutTeachers);
                            query = query.Where(d => finalDepartmentIds.Contains(d.Id));
                        }
                        else
                        {
                            // Стандартная фильтрация
                            var departmentIdsQuery = departmentCounts.AsQueryable();

                            if (filter.MinTeachersCount.HasValue)
                                departmentIdsQuery = departmentIdsQuery.Where(x => x.Count >= filter.MinTeachersCount.Value);

                            if (filter.MaxTeachersCount.HasValue)
                                departmentIdsQuery = departmentIdsQuery.Where(x => x.Count <= filter.MaxTeachersCount.Value);

                            var departmentIds = departmentIdsQuery
                                .Select(x => x.DepartmentId)
                                .ToList();

                            query = query.Where(d => departmentIds.Contains(d.Id));
                        }
                    }
                }

                return await query.ToArrayAsync(cancellationToken);
            }

            public async Task<Department?> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken)
            {
                return await _dbContext.Departments
                    .Include(d => d.Head)
                    .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
            }

            public async Task<Department> AddDepartmentAsync(Department department, CancellationToken cancellationToken)
            {
                _dbContext.Departments.Add(department);
                await _dbContext.SaveChangesAsync(cancellationToken);

                // Возвращаем с загруженными зависимостями
                return await _dbContext.Departments
                    .Include(d => d.Head)
                    .FirstOrDefaultAsync(d => d.Id == department.Id, cancellationToken);
            }

            public async Task<Department> UpdateDepartmentAsync(UpdateDepartmentDto departmentDto, CancellationToken cancellationToken)
            {
                var existingDepartment = await _dbContext.Departments
                    .FirstOrDefaultAsync(d => d.Id == departmentDto.Id, cancellationToken);

                if (existingDepartment == null)
                    throw new ArgumentException("Кафедра не найдена");

                // Проверка существования заведующего, если указан
                if (departmentDto.HeadId.HasValue)
                {
                    var headExists = await _dbContext.Teachers
                        .AnyAsync(t => t.Id == departmentDto.HeadId.Value, cancellationToken);

                    if (!headExists)
                        throw new ArgumentException("Указанный заведующий не существует");
                }

                // Обновляем поля
                existingDepartment.Name = departmentDto.Name;
                existingDepartment.FoundedDate = departmentDto.FoundedDate;
                existingDepartment.HeadId = departmentDto.HeadId;

                _dbContext.Departments.Update(existingDepartment);
                await _dbContext.SaveChangesAsync(cancellationToken);

                // Возвращаем обновленную кафедру с загруженными зависимостями
                return await _dbContext.Departments
                    .Include(d => d.Head)
                    .FirstOrDefaultAsync(d => d.Id == departmentDto.Id, cancellationToken);
            }

            public async Task<bool> DeleteDepartmentAsync(int id, CancellationToken cancellationToken)
            {
                // Удаление преподавателей кафедры через отдельный запрос
                var teachersToDelete = await _dbContext.Teachers
                    .Where(t => t.DepartmentId == id)
                    .ToListAsync(cancellationToken);

                if (teachersToDelete.Any())
                {
                    _dbContext.Teachers.RemoveRange(teachersToDelete);
                }

                var department = await _dbContext.Departments
                    .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

                if (department == null) return false;

                _dbContext.Departments.Remove(department);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}
