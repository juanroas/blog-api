using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Blog.Domain.Models
{
    public class User : IdentityUser
    {
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}