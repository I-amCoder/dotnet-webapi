using AutoMapper;
using api_with_auth.Responses;
using Microsoft.AspNetCore.Mvc;
using api_with_auth.Models;
using api_with_auth.Repository.IRepository;
using api_with_auth.Models.Dto;
using System.Net;

namespace api_with_auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _repository;
        private ApiResponse _response;
        public PostsController(IPostRepository repository,IMapper mapper) 
        { 
            _mapper = mapper;
            _repository = repository;
            _response = new();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            try
            {
                IEnumerable<Post> posts = await _repository.GetAllAsync();
                _response.Result = _mapper.Map<List<PostDto>>(posts);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            } catch(Exception ex)
            {
                _response.ErrorMessages = new List<string> { ex.Message.ToString() };
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<ApiResponse>> Get(int Id)
        {
            if(Id == null || Id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            Post post = await _repository.GetAsync(u=>u.Id  == Id);
            
            if(post == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<PostDto>(post);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response );
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create(CreatePostDto createDto)
        {
            if(createDto == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            
            try
            {
                Post post = _mapper.Map<Post>(createDto);
                await _repository.CreateAsync(post);
                _response.Result = _mapper.Map<PostDto>(post);

                return CreatedAtAction(nameof(Get), new { post.Id }, post);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = [ ex.Message.ToString() ];
                return BadRequest(_response);
            }
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult<ApiResponse>> Update(int Id)
        {
            if(Id == 0)
            {
                return BadRequest();
            }

            try
            {
                Post post = await _repository.GetAsync(p => p.Id == Id);
                if(post == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                

            }catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = [ex.Message.ToString()];
            }

            return _response;
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult<ApiResponse>> deletePost(int Id)
        {
            try
            {
                if(Id == 0)
                {
                    return BadRequest();
                }

                var post = await _repository.GetAsync(e => e.Id == Id);
                if(post == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;    
                    return NotFound(_response);
                }

                await _repository.RemoveAsync(post);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);

            }catch(Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages = [ex.Message.ToString() ];
                return _response;
            }
        }

    }
}
