using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using DALLib.Models;
using ASPNETAPP.ViewModels;

namespace ASPNETAPP.AutoMapperConfiguration
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {

            CreateMap<Instance, InstanceViewModel>()
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.InstanceVersion != null ? 
                $"{src.InstanceVersion.Version}" : String.Empty))
                .ForMember(dest => dest.DatabasesCount, opt => opt.MapFrom(src => src.Databases.Count))
                .ForMember(dest => dest.Alias, opt => opt.Ignore())
                .ForMember(dest => dest.IsHidden, opt => opt.Ignore())
                .ForMember(dest => dest.IsWindowsAuthentication, opt => opt.Ignore())
                .ForMember(dest => dest.Login, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<InstanceViewModel, Instance>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Convert.ToInt32(src.Id)))
                .ForMember(dest => dest.Authentication, opt => opt.MapFrom(src =>
                Convert.ToBoolean(src.IsWindowsAuthentication) ? AuthenticationType.Windows : AuthenticationType.Sql))
                .ForMember(dest => dest.Assigns, opt => opt.Ignore())
                .ForMember(dest => dest.Databases, opt => opt.Ignore())
                .ForMember(dest => dest.InstVersionId, opt => opt.Ignore())
                .ForMember(dest => dest.InstanceVersion, opt => opt.Ignore())
                .ForMember(dest => dest.EncryptionKey, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeletedTime, opt => opt.Ignore());

            CreateMap<Database, DatabaseViewModel>();

            CreateMap <DbUser, DatabaseUserViewModel> ()
            .ForMember(dest => dest.AssignedRoles, opt => opt.MapFrom(src => src.Roles.Select(role => role.Id))); 

            CreateMap <DbRole, DatabaseRoleViewModel> ()
            .ForMember(dest => dest.AssignedUsers, opt => opt.MapFrom(src => src.Users.Select(user => user.Id)));

            CreateMap<DbPrincipal, DatabasePrincipalViewModel> ();

            CreateMap<User, UserViewModel>();


            CreateMap<InstRole, InstanceRoleViewModel>()
            .ForMember(dest => dest.AssignedLoginsIds, opt => opt.MapFrom(src => src.Logins.Select(L => L.Id)));
            
            CreateMap<InstPrincipal, InstancePrincipalViewModel> ();

            
            CreateMap <InstLogin, InstanceLoginViewModel> ()
            .ForMember(dest => dest.AssignedRolesIds, opt => opt.MapFrom(src => src.Roles.Select(R => R.Id)));

            CreateMap <InstPermission, PermissionViewModel> ();

            CreateMap<DbPermission, PermissionViewModel>();

            CreateMap<NewInstanceViewModel, Instance>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Convert.ToInt32(src.Id)))
                .ForMember(dest => dest.Authentication, opt => opt.MapFrom(src =>
                Convert.ToBoolean(src.IsWindowsAuthentication) ? AuthenticationType.Windows : AuthenticationType.Sql))
                .ForMember(dest => dest.Assigns, opt => opt.Ignore())
                .ForMember(dest => dest.Databases, opt => opt.Ignore())
                .ForMember(dest => dest.CpuCount, opt => opt.Ignore())
                .ForMember(dest => dest.InstanceVersion, opt => opt.Ignore())
                .ForMember(dest => dest.InstVersionId, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.IsOK, opt => opt.Ignore())
                .ForMember(dest => dest.Memory, opt => opt.Ignore())
                .ForMember(dest => dest.OSVersion, opt => opt.Ignore())
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Logins, opt => opt.Ignore())
                .ForMember(dest => dest.EncryptionKey, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeletedTime, opt => opt.Ignore());

        }
    }
}

       