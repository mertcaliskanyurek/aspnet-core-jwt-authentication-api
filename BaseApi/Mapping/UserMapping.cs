using System;
using AutoMapper;
using BaseApi.Domain.Entity.Model;
using BaseApi.Resources;

namespace BaseApi.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<UserResource, User>();
            CreateMap<User, UserResource>();
        }
    }
}
