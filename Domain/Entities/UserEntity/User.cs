using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.UserEntity;

public class User : IdentityUser<string>
{
    public string PNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime BirthDate{ get; set; }
   
}
