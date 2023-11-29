using System.Threading.Tasks;
using AuthService.Domain.Models;
using Shared.Models.Domain.Users;
using Shared.Models.Infrastructure.Authorization;

namespace AuthService.Domain;

public interface IUserRepository
{
    Task<CreateUserResult> CreateUser(string email, string password, string name);
    Task<RemoveUserResult> RemoveUser(UserId id);
    Task<SaveUserResult> SaveUser(UserEntity user);
    Task<UserEntity> GetUser(UserId userId);
    Task<UserEntity> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(UserEntity user, string password);
    Task SetAuthenticationTokenAsync(UserEntity user, AuthorizationToken token);
    Task<string> GetRoleId(Role role);
    Task<bool> RoleExists(Role role);
    Task<CreateRoleResult> CreateRole(Role role);
    Task<int> GetUsersCountWithRole(Role role);
}

public record CreateUserResult(bool Success, string Error, UserEntity CreatedUser);

public record RemoveUserResult(bool Success, string Error, UserEntity RemovableUser);

public record SaveUserResult(bool Success, string Error);

public record CreateRoleResult(bool Success, string Error);