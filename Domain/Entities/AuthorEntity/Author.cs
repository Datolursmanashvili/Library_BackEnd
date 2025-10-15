using Domain.Shared.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.AuthorEntity;

public class Author : BaseEntity<int>
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; }  

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; }  

    [Required]
    [RegularExpression("^(კაცი|ქალი)$", ErrorMessage = "სქესი უნდა იყოს 'კაცი' ან 'ქალი'.")]
    public string Gender { get; set; }  

    [Required]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "პირადი ნომერი უნდა შეიცავდეს 11 ციფრს.")]
    public string PersonalNumber { get; set; }  

    [Required]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(Author), nameof(ValidateAge))]
    public DateTime BirthDate { get; set; }  

    [Required]
    public int CountryId { get; set; }  

    [Required]
    public int CityId { get; set; }  

    [StringLength(50, MinimumLength = 4)]
    public string PhoneNumber { get; set; }  

    [EmailAddress]
    public string Email { get; set; }  

    // Custom validation method for age
    public static ValidationResult ValidateAge(DateTime birthDate, ValidationContext context)
    {
        var age = DateTime.Today.Year - birthDate.Year;
        if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;
        return age >= 18
            ? ValidationResult.Success
            : new ValidationResult("ავტორი უნდა იყოს მინიმუმ 18 წლის.");
    }
}
