using Domain.Shared.BaseModel;

namespace Domain.Entities.BookAuthorEntity;

public class BookAuthor : BaseEntity<int>
{
    public int ProductId { get; set; }
    public int AuthorId { get; set; }

    // Navigation properties
    public virtual Domain.Entities.ProductEntity.Product Product { get; set; }
    public virtual Domain.Entities.AuthorEntity.Author Author { get; set; }
}
