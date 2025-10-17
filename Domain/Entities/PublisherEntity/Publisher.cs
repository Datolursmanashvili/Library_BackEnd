using Domain.Shared.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.PublisherEntity;

public class Publisher : BaseEntity<int>
{
    [Required]
    [StringLength(250, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}
