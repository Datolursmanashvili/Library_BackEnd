using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.RoleEntity;

public class ApplicationRole : IdentityRole<string>
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();

    public override string Name { get; set; }
    public IEnumerable<Permissions>? Permissions { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public DateTime? DeletedAt { get; set; } = null;

    public bool IsDeleted { get; set; } = false;
}
public enum Permissions
{
    All = 1,
}