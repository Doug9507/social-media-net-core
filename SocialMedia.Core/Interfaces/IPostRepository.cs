﻿using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetPosts();
        Task<Post> GetPostId(int PostId);
        Task SavePost(Post post);
        Task<bool> DeletePost(int PostId);
        Task<bool> EditPost(Post post);

    }
}