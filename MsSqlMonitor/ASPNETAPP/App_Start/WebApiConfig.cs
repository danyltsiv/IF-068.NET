using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using ASPNETAPP.Unity;
using CommonLib;
using DALLib;
using DALLib.Contracts;
using DALLib.EF;
using Microsoft.Practices.Unity;
using DALLib.Models;
using System.Web.Http.Filters;
using ASPNETAPP.DataProvider;
using AutoMapper;
using ASPNETAPP.AutoMapperConfiguration;
using SQLInfoCollectorService.Security;
using System.Web.Http.Dispatcher;
using Unity.WebApi;

namespace ASPNETAPP
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            var container = new UnityContainer();

            config.Services.Replace(typeof(IHttpControllerActivator), new ServiceActivator(container));
            container.RegisterInstance<IHttpControllerActivator>(new ServiceActivator(container));

            var mapperСonfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
                //cfg.AddProfile<MappingProfieForNewModels>();
                //cfg.CreateMap<Source, Dest>();
            });
            //var mapper = mapperСonfig.CreateMapper();
            //mapperСonfig.AssertConfigurationIsValid();//check if mapping is ok
            IMapper mapper = new Mapper(mapperСonfig);    
            container.RegisterInstance<IMapper>(mapper);

            container.RegisterType<IEncryption, Encryption>();
            container.RegisterType<IEncryptionManager, EncryptionManager>();




            container.RegisterType<ISLogger, SLogger>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IUnitOfWork, UnitOfWork>(new InjectionConstructor(
                 typeof(MsSqlMonitorEntities),
                 new ApplicationUserManager(new UserStore(new MsSqlMonitorEntities())),
                 new ApplicationRoleManager(new RoleStore(new MsSqlMonitorEntities()))
                 ));
            container.RegisterType<IMonitorDataProvider, LocalDbDataProvider>();

            // config.DependencyResolver = new UnityResolver(container);
            config.DependencyResolver =  new UnityDependencyResolver(container);



             var providers = config.Services.GetFilterProviders().ToList();

            var defaultprovider = providers.Single(i => i is ActionDescriptorFilterProvider);
            config.Services.Remove(typeof(System.Web.Http.Filters.IFilterProvider), defaultprovider);

            config.Services.Add(typeof(System.Web.Http.Filters.IFilterProvider), new UnityFilterProvider(container));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            GlobalConfiguration.Configuration.Formatters
                .JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Enforce HTTPS
            config.Filters.Add(new ASPNETAPP.ActionFilters.RequireHttpsAttribute());
        }
    }
}
