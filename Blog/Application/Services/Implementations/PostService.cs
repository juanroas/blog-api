using Blog.Application.Services.Interfaces;
using Blog.Domain.Models;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly MySqlBlogContext _context;
        private readonly NotificationService _notificationService;

        public PostService(MySqlBlogContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<List<Post>> GetAllPosts() => await _context.Posts.ToListAsync();

        public async Task<Post> GetPost(int id) => await _context.Posts.FindAsync(id);

        public async Task<Post> CreatePost(string title, string content, string userId)
        {
            var post = new Post { Title = title, Content = content, UserId = userId, CreatedAt = DateTime.UtcNow };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            await _notificationService.NotifyNewPost($"Nova postagem criada: {post.Title}");

            return post;
        }


        public async Task UpdatePost(int postId, string userId, string title, string content)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId && p.UserId == userId);
            if (post == null)
                throw new InvalidOperationException("Post não encontrado ou você não tem permissão para editá-lo.");

            post.Title = title;
            post.Content = content;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePost(int postId, string userId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId && p.UserId == userId);
            if (post == null)
                throw new InvalidOperationException("Post não encontrado ou você não tem permissão para excluí-lo.");

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }


    }
}
