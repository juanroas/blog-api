namespace Blog.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterUser(string email, string password);
        Task<bool> LoginUser(string email, string password);
    }
}
