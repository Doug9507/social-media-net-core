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

        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _unitOfWork.PostRepository.GetAll();
        }
        public async Task<Post> GetPostId(int PostId)
        {
            return await _unitOfWork.PostRepository.GetById(PostId);
        }

        public async Task SavePost(Post post)
        {
            var user =  await _unitOfWork.UserRepository.GetById(post.UserId);

            if (user == null)
            {
                throw new Exception("El usuario no existe");
            }
             await _unitOfWork.PostRepository.Add(post);
        }

        public async Task<bool> EditPost(Post post)
        {
             await _unitOfWork.PostRepository.Update(post);
            return true;
        }
        public async Task<bool> DeletePost(int PostId)
        {
             await _unitOfWork.PostRepository.Delete(PostId);
            return true;
        }
    }
}
