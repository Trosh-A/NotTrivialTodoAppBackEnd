using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using TodoApp.Backend.Models.UserModel;

namespace TodoApp.Backend.Models.AutoMapperProfiles;

public class UserProfile : Profile
{
  public UserProfile()
  {
    CreateMap<IdentityUser<Guid>, UserReadDto>()
      .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Id))
      .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));
  }
}
