using AutoMapper;
using CoreAdminLTE.Data;
using CoreAdminLTE.Models;

namespace CoreAdminLTE.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterModel, User>();
        }
    }
}