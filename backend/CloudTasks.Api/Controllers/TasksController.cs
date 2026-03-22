using CloudTasks.Api.Data;
using CloudTasks.Api.Dtos;
using CloudTasks.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CloudTasks.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }
    /// <summary>GET /api/tasks — lista wyłącznie jako DTO (bez encji EF).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TaskReadDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TaskReadDto>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _context.Tasks
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);

        IReadOnlyList<TaskReadDto> dto = items.Select(ToReadDto).ToList();
        return Ok(dto);
    }
    /// <summary>GET /api/tasks/{id} — pojedyncze zadanie jako DTO.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskReadDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (entity is null)
            return NotFound();

        return Ok(ToReadDto(entity));
    }
    /// <summary>POST /api/tasks — walidacja body (DTO), zapis encji, odpowiedź jako DTO.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaskReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskReadDto>> Create([FromBody] TaskCreateDto body, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);
        var name = body.Name.Trim();
        if (string.IsNullOrEmpty(name))
            return BadRequest("Name is required.");
        var entity = new TaskEntity
        {
            Name = name,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };
        _context.Tasks.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        var dto = ToReadDto(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
    }
    private static TaskReadDto ToReadDto(TaskEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        IsCompleted = entity.IsCompleted
    };
}
