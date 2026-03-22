namespace CloudTasks.Api.Dtos;

/// <summary>Kontrakt API — bez pól wewnętrznych bazy (np. CreatedAt można ukryć lub udostępnić wg potrzeb).</summary>
public class TaskReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
