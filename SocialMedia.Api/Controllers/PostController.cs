using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK,Type =typeof(ApiResponse<IEnumerable<PostDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<PostDTO>>))]
        public IActionResult GetPosts([FromQuery] PostQueryFilter postQueryFilter)
        {
            var posts =  _postService.GetPosts(postQueryFilter);

            var postsDTO = _mapper.Map<IEnumerable<PostDTO>>(posts);

            var response = new ApiResponse<IEnumerable<PostDTO>>(postsDTO);

            var metadata = new
            {
              posts.PageNumber,
              posts.PageSize,
              posts.HasPreviousPage,
              posts.HasNextPage,
              posts.NextPageNumber,
              posts.PreviousPageNumber,
              posts.TotalPages
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        [HttpGet("{id}")]
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

            
            return Ok(response);
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
            postDomain.Id = id;

            var result = await _postService.EditPost(postDomain);

            var response = new ApiResponse<bool>(result);

            return Ok(response);
        }
    }
}
