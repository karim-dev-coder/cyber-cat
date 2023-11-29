using AuthService.Domain;
using AuthService.Domain.Models;
using Shared.Models.Domain.Users;
using Shared.Models.Infrastructure.Authorization;

namespace AuthService.Tests.Mocks;

internal class MockUserRepository : IUserRepository
{
    private readonly Dictionary<string, UserEntity> _users = new();
    private readonly Dictionary<string, RoleEntity> _roles = new();

    public async Task<UserEntity> FindByEmailAsync(string email)
    {
        return _users.Values.FirstOrDefault(user => user.Email == email);
    }

    public async Task<bool> CheckPasswordAsync(UserEntity user, string password)
    {
        return user.PasswordHash == password.GetHashCode().ToString();
    }

    public async Task SetAuthenticationTokenAsync(UserEntity user, AuthorizationToken token)
    {
        Console.WriteLine($"Set access token '{token.Value}' for user '{user.Id}'");
    }

    public async Task<CreateUserResult> CreateUser(string email, string password, string name)
    {
        var user = new UserEntity()
        {
            Id = (_users.Count + 1).ToString(),
            UserName = name,
            Email = email,
            PasswordHash = password.GetHashCode().ToString()
        };
        _users[user.Id] = user;
        return new CreateUserResult(true, string.Empty, user);
    }

    public async Task<RemoveUserResult> RemoveUser(UserId userId)
    {
        var user = _users[userId.Value.ToString()];
        var success = _users.Remove(userId.ToString());

        return new RemoveUserResult(success, string.Empty, user);
    }

    public async Task<int> GetUsersCountWithRole(Role role)
    {
        return 1;
    }

    public async Task<UserEntity> GetUser(UserId userId)
    {
        return _users[userId.Value.ToString()];
    }

    public async Task<SaveUserResult> SaveUser(UserEntity user)
    {
        var success = _users.Remove(user.Id);
        _users[user.Id] = user;

        return new SaveUserResult(success, string.Empty);
    }

    public async Task<string> GetRoleId(Role role)
    {
        return _roles[role.Id].Id;
    }

    public async Task<bool> RoleExists(Role role)
    {
        return _roles.ContainsKey(role.Id);
    }

    public async Task<CreateRoleResult> CreateRole(Role role)
    {
        _roles[role.Id] = new RoleEntity()
        {
            Id = role.Id
        };

        return new CreateRoleResult(true, string.Empty);
    }
}