using Microsoft.AspNetCore.Mvc;
using MatveevVadimKt_42_22.Filters.DisciplineFilter;
using MatveevVadimKt_42_22.Interfaces.DisciplineInterfaces;
using MatveevVadimKt_42_22.Models;
using MatveevVadimKt_42_22.Database;
using MatveevVadimKt_42_22.Models.DTO;
using Microsoft.EntityFrameworkCore;


namespace MatveevVadimKt_42_22.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisciplinesByHeadController : ControllerBase
    {
        private readonly UniversityDbContext _context;

        public DisciplinesByHeadController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: api/DisciplinesByHead?lastName=Иванов
        [HttpGet]
        public async Task<IActionResult> GetDisciplinesByHeadLastName([FromQuery] string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                return BadRequest("Не указана фамилия заведующего.");

            var disciplines = await (
                from d in _context.Departments
                join h in _context.Teachers on d.HeadId equals h.Id
                join t in _context.Teachers on d.Id equals t.DepartmentId
                join l in _context.Loads on t.Id equals l.TeacherId
                join dis in _context.Disciplines on l.DisciplineId equals dis.Id
                where h.LastName == lastName
                select new DisciplineDto1
                {
                    Discipline = dis.Name,
                    Department = d.Name,
                    Head = h.LastName
                }
            ).Distinct()
            .ToListAsync();

            if (disciplines.Count == 0)
                return NotFound("Кафедра с таким заведующим не найдена или нет дисциплин.");

            return Ok(disciplines);
        }

    }
}