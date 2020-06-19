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

        private readonly IRepository<Post> _postrepository;
        private readonly IRepository<User> _userRepository;

        public PostService(IRepository<Post> postrepository, IRepository<User> userrepository)
        {
            _postrepository = postrepository;
            _userRepository = userrepository;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _postrepository.GetAll();
        }
        public async Task<Post> GetPostId(int PostId)
        {
            return await _postrepository.GetById(PostId);
        }

        public async Task SavePost(Post post)
        {
            var user =  await _userRepository.GetById(post.UserId);

            if (user == null)
            {
                throw new Exception("El usuario no existe");
            }
             await _postrepository.Add(post);
        }

        public async Task<bool> EditPost(Post post)
        {
             await _postrepository.Update(post);
            return true;
        }
        public async Task<bool> DeletePost(int PostId)
        {
             await _postrepository.Delete(PostId);
            return true;
        }
    }
}
