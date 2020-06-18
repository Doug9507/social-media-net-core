using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly SocialMediaContext _context;
        public PostRepository(SocialMediaContext context)
        {
            _context = context;
        }

        public async Task SavePost(Post post)
        {
            await _context.Posts.AddAsync(post);

            _context.SaveChanges();
        }

        public async Task<Post> GetPostId(int PostId)
        {
            var post = await _context.Posts.FindAsync(PostId);

            return post;
        }
        public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            var postordered = from q in posts orderby q.PostId descending select q;

            return postordered;
        }
        public async Task<bool> EditPost(Post post)
        {
            //_context.Entry(post).State = EntityState.Modified;
            var postFind = await GetPostId(post.PostId);
            postFind.UserId = post.UserId;
            postFind.Description = post.Description;
            postFind.Date = post.Date;
            postFind.Image = post.Image;

            var row = await _context.SaveChangesAsync();

            return row > 0;
        }
        public async Task<bool> DeletePost(int PostId)
        {
            //var postdelete =  await _context.Posts.FindAsync(PostId);
            var postdelete = await GetPostId(PostId);

            _context.Remove(postdelete);

            var row = await _context.SaveChangesAsync();

            return row > 0;
        }
    }
}
