using Application.Shared;
using Microsoft.AspNetCore.Http;
using Shared;

namespace Application.Commands.UserCommands;

/// <summary>
/// Register a new user (POST JSON). Default role name: <c>user</c>.
/// </summary>
public class RegistrationCommand : Command<UserCommandResult>
{
    /// <summary>Desired login username.</summary>
    public string Username { get; set; }

    /// <summary>Plain-text password (hashed server-side).</summary>
    public string Password { get; set; }

    /// <summary>Unique email (login).</summary>
    public string Email { get; set; }

    /// <summary>Personal identification number (stored encrypted).</summary>
    public string PNumber { get; set; }

    /// <summary>First name.</summary>
    public string FirstName { get; set; }

    /// <summary>Last name.</summary>
    public string LastName { get; set; }

    /// <summary>Contact phone.</summary>
    public string Phone { get; set; }

    /// <summary>Department id.</summary>
    public int DepartmentId { get; set; }

    /// <summary>Date of birth.</summary>
    public DateTime BirthDate { get; set; }

    /// <inheritdoc />
    public override async Task<CommandExecutionResultGeneric<UserCommandResult>> ExecuteCommandLogicAsync()
    {
        if (!PasswordHelper.IsValidEmail(Email))
        {
            return await Fail<UserCommandResult>(StatusCodes.Status400BadRequest, "მეილის ფორმატი არასწორია");
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
            var code = res.Code is >= 400 and < 600 ? res.Code : StatusCodes.Status400BadRequest;
            return await Fail<UserCommandResult>(code, res.ErrorMessage);
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
        }, StatusCodes.Status201Created);
    }

}


/// <summary>Payload returned after successful registration.</summary>
public class UserCommandResult
{
    /// <summary>Email on the created account.</summary>
    public string Email { get; set; }

    /// <summary>Whether the account is active.</summary>
    public bool IsActive { get; set; }

    /// <summary>New user id.</summary>
    public string UserID { get; set; }

    /// <summary>Stored personal number (may be encrypted form).</summary>
    public string PNumber { get; set; }

    /// <summary>First name.</summary>
    public string FirstName { get; set; }

    /// <summary>Last name.</summary>
    public string LastName { get; set; }

    /// <summary>Date of birth.</summary>
    public DateTime BirthDate { get; set; }
}
