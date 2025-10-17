using Domain.Shared.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.LocationEntity;

public class Location : BaseEntity<int>
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    // True = Country, False = City
    public bool IsCountry { get; set; }

    // for City — link to Country
    public int? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    public Location? Parent { get; set; }

    public ICollection<Location>? Children { get; set; }
}
