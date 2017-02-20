using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using DALLib.Models;

namespace ASPNETAPP.ViewModels
{
    public class InstanceModelA
    {
        public int? Id { get; set; }

        public string ServerName { get; set; }

        public string InstanceName { get; set; }

        public string Status { get; set; }

        public string OSVersion { get; set; }

        public byte? CpuCount { get; set; }

        public int? Memory { get; set; }

        public bool? IsOK { get; set; }

        public bool? IsDeleted { get; set; }

        public string Version { get; set; }

        //public List<InstanceLoginViewModel> Logins { get; set; } = new List<InstanceLoginViewModel>();

        //public List<InstanceRoleViewModel> Roles { get; set; } = new List<InstanceRoleViewModel>();

        public int? DatabasesCount { get; set; }
    }
    public class MappingProfieForInstanceModelA : Profile
    {
        protected override void Configure()
        {
            CreateMap<Instance, InstanceModelA>()
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.InstanceVersion != null ? $"{src.InstanceVersion.Version}" : String.Empty))
                .ForMember(dest => dest.DatabasesCount, opt => opt.MapFrom(src => src.Databases.Count()));
            //DatabasesCount = instance.Databases?.Count
        }
    }

}



