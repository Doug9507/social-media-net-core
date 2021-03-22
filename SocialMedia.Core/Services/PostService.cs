using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
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
        private readonly PaginationOptions _options;

        public PostService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _options = options.Value;
        }

        public PagedList<Post> GetPosts(PostQueryFilter postQueryFilter)
        {
            postQueryFilter.PageNumber = postQueryFilter.PageNumber == 0 ? _options.DefaultPageNumber : postQueryFilter.PageSize;
            postQueryFilter.PageSize = postQueryFilter.PageSize == 0 ? _options.DefaultPageSize : postQueryFilter.PageSize;

            var posts =  _unitOfWork.PostRepository.GetAll();

            if (postQueryFilter.UserId != null)
            {
                posts = posts.Where(x=>x.UserId==postQueryFilter.UserId);
            }

            if (postQueryFilter.Date != null)
            {
                posts = posts.Where(x => x.Date.ToShortDateString() == postQueryFilter.Date?.ToShortDateString());
            }

            if (postQueryFilter.Description != null)
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(postQueryFilter.Description.ToLower()));
            }

            var pagedPosts = PagedList<Post>.Create(posts,postQueryFilter.PageNumber,postQueryFilter.PageSize);

            return pagedPosts;
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
