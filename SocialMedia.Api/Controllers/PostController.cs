﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Responses;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        //private readonly IPostRepository _postRepository;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postService.GetPosts();

            //var postsDTO = posts.Select(x => new PostDTO { 
            //              PostId = x.PostId,
            //              UserId = x.UserId,
            //              Description = x.Description,
            //              Date = x.Date,
            //              Image = x.Image
            //});

            var postsDTO = _mapper.Map<IEnumerable<PostDTO>>(posts);

            var response = new ApiResponse<IEnumerable<PostDTO>>(postsDTO);

            return Ok(response);
        }

        [HttpGet("{PostId}")]
        public async Task<IActionResult> GetPostId(int id)
        {
            var post = await _postService.GetPostId(id);

            var postDTO = _mapper.Map<PostDTO>(post);

            var response = new ApiResponse<PostDTO>(postDTO);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SavePost(PostDTO postDTO)
        {
            var post = _mapper.Map<Post>(postDTO);

            await _postService.SavePost(post);

            postDTO = _mapper.Map<PostDTO>(post);

            var response = new ApiResponse<PostDTO>(postDTO);

            
            return Ok(postDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _postService.DeletePost(id);

            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> EditPost(int id, PostDTO postDTO)
        {
            var postDomain = _mapper.Map<Post>(postDTO);
            postDomain.PostId = id;

            var result = await _postService.EditPost(postDomain);

            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }
    }
}