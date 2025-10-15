namespace Domain.Shared.BaseModel;

public class BaseEntity<T>
{
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

}