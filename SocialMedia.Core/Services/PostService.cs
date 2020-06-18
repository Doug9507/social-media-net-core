using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postrepository;
        private readonly IUserRepository _userRepository;

        public PostService(IPostRepository postrepository, IUserRepository userRepository)
        {
            _postrepository = postrepository;
            _userRepository = userRepository;
        }

        public async  Task<bool> DeletePost(int PostId)
        {
            return await _postrepository.DeletePost(PostId);
        }

        public async Task<bool> EditPost(Post post)
        {
           return  await _postrepository.EditPost(post);
        }

        public async Task<Post> GetPostId(int PostId)
        {
            return await _postrepository.GetPostId(PostId);
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _postrepository.GetPosts();
        }

        public async Task SavePost(Post post)
        {
            var user =  await _userRepository.GetUser(post.UserId);

            if (user == null)
            {
                throw new Exception("El usuario no existe");
            }
             await _postrepository.SavePost(post);
        }
    }
}
