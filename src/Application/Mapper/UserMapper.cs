using AutoMapper;
using Domain.Entities.Identities;
using Domain.Enums;
using ShareLoanApp.Application.DTOs;
using ShareLoanApp.Domain.DTOs;
using ShareLoanApp.Domain.Entities;

namespace ShareLoanApp.Application.Mapper
{
    public class UserMapper: Profile
    {
        public UserMapper()
        {
            CreateMap<User, CreateUserResponse>();

            CreateMap<CreateUserDTO, User>().AfterMap((src, dest) =>
            {
                dest.Email = src.Email.Trim().ToLower();
                dest.UserName = dest.Email;
                dest.Status = EUserStatus.ACTIVE.ToString();
            });

            CreateMap<User, UserLoginResponse>().ReverseMap();

            CreateMap<User, UserByIdResponse>();
        }
    }
}
