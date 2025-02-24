using Blog.Domain.Models;

namespace Blog.Application.Services.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPosts();
        Task<Post> GetPost(int id);
        Task<Post> CreatePost(string title, string content, string userId);
        Task UpdatePost(int postId, string userId, string title, string content);
        Task DeletePost(int postId, string userId);
    }
}
