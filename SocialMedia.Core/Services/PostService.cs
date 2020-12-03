using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Post> GetPosts()
        {
            return  _unitOfWork.PostRepository.GetAll();
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
                //throw new Exception("El usuario no existe");
                throw new BusinessLogic("El usuario no existe");
            }

            var postsByUser = await _unitOfWork.PostRepository.GetPostsByUser(user.Id);

            
            if (postsByUser.Count() < 10)
            {
                var lastPost = postsByUser.OrderByDescending(x => x.Date).FirstOrDefault();

                if ((DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    //throw new Exception("No esta habilitado para publicar");
                    throw new BusinessLogic("No esta habilitado para publicar");
                }
            }

             await _unitOfWork.PostRepository.Add(post);
             await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> EditPost(Post post)
        {
            _unitOfWork.PostRepository.Update(post);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeletePost(int PostId)
        {
             await _unitOfWork.PostRepository.Delete(PostId);
             await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
