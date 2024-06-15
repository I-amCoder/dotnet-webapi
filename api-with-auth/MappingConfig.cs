namespace api_with_auth
{
    using api_with_auth.Models;
    using api_with_auth.Models.Dto;
    using AutoMapper;
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();
            
            CreateMap<Post,CreatePostDto>().ReverseMap();
        }
    }
}
