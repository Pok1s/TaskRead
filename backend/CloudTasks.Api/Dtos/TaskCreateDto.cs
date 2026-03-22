using System.ComponentModel.DataAnnotations;

namespace CloudTasks.Api.Dtos;

public class TaskCreateDto
{
    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;
}
