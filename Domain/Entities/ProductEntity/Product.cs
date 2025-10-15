using Domain.Shared.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.ProductEntity;

public class Product : BaseEntity<int>
{
    [Required]
    [StringLength(250, MinimumLength = 2)]
    public string Name { get; set; }  

    [Required]
    [StringLength(500, MinimumLength = 100)]
    public string Annotation { get; set; }  // ანოტაცია

    [Required]
    [RegularExpression("^(წიგნი|სტატია|ელექტრონული რესურსი)$", ErrorMessage = "პროდუქტის ტიპი უნდა იყოს 'წიგნი', 'სტატია' ან 'ელექტრონული რესურსი'.")]
    public string ProductType { get; set; }  // პროდუქტის ტიპი

    [Required]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "ISBN უნდა შეიცავდეს ზუსტად 13 ციფრს.")]
    public string ISBN { get; set; }  // ISBN

    [Required]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }  // გამოშვების თარიღი

    [Required]
    public int PublisherId { get; set; }  // გამომცემლობა (foreign key)

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "გვერდების რაოდენობა უნდა იყოს მინიმუმ 1.")]
    public int PageCount { get; set; }  // გვერდების რაოდენობა

    [Required]
    [StringLength(500)]
    public string Address { get; set; }  // მისამართი (წიგნის შემთხვევაში ფიზ. მისამართი ბიბლიოთეკაში, სტატიის ან ელ. რესურსის შემთხვევაში URL)
}
