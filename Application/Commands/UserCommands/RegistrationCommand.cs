using Application.Shared;
using Shared;

namespace Application.Commands.UserCommands;

public class RegistrationCommand : Command<UserCommandResult>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string PNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public int DepartmentId { get; set; }
    public DateTime BirthDate { get; set; }

    public override async Task<CommandExecutionResultGeneric<UserCommandResult>> ExecuteCommandLogicAsync()
    {
        if (!PasswordHelper.IsValidEmail(Email))
        {
            return await Fail<UserCommandResult>("მეილის ფორმატი არასწორია");
        }
        var res = await userRepository.Registration(new Domain.Entities.UserEntity.User()
        {
            UserName = Username,
            PasswordHash = Password,
            Email = Email,
            PNumber = PNumber,
            FirstName = FirstName,
            LastName = LastName,
            PhoneNumber = Phone,
            DepartmentId = DepartmentId,
            BirthDate = BirthDate,
        }, "user");

        if (res.Success == false)
        {
            return await Fail<UserCommandResult>(res.ErrorMessage);
        }

        var user = applicationDbContext.Users.FirstOrDefault(x => x.Email == Email);

        return await Ok(new UserCommandResult()
        {
            UserID = user.Id,
            Email = user.Email,
            BirthDate = user.BirthDate,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PNumber = user.PNumber,
            IsActive = user.IsActive,
        });
    }

}


public class UserCommandResult
{
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public string UserID { get; set; }
    public string PNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
}

