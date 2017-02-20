using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SQLInfoCollectionService.Collectors;
using SQLInfoCollectionService.Contracts;
using System.Data.SqlClient;
using System.Data;
using SQLInfoCollectionService.Entities;
using SQLInfoCollectionService.InstanceInfoUpdating;
using DALLib.Contracts;
using DALLib;
using CommonLib;
using DALLib.EF;
using DALLib.Models;
using SQLInfoCollectorService.Security;

namespace SQLInfoCollectionService.Configuration
{
    public class DependencyConfig
    {
        public static IUnityContainer Initialize()
        {

            IUnityContainer container = new UnityContainer();

            container.RegisterType<IEncryption, Encryption>();
            container.RegisterType<IEncryptionManager, EncryptionManager>();
            container.RegisterType<ISLogger, SLogger>();
            container.RegisterType<IDbConnection, SqlConnection>();
            container.RegisterType<IResourceManager, ResourceManager>();
            container.RegisterType<IConnectionManager, ConnectionManager>(new ContainerControlledLifetimeManager());

            container.RegisterType<IUnitOfWork, UnitOfWork>();
            //  container.RegisterType<IInstanceDataCollector, InstanceDataCollector>();

            container.RegisterType<ILocalStorage, LocalDb>();

            container.RegisterType<IInstanceDataCollector, SQLInfoCollectionService.Collectors.InstanceDataCollector>(
                                                                        new InjectionConstructor(typeof(IConnectionManager), typeof(IResourceManager), typeof(ISLogger)));

            container.RegisterType<IUnitOfWork, UnitOfWork>(new InjectionConstructor(
                typeof(MsSqlMonitorEntities),
                new ApplicationUserManager(new UserStore(new MsSqlMonitorEntities())),
                new ApplicationRoleManager(new RoleStore(new MsSqlMonitorEntities()))
                ));

            UnityServiceLocator serviceProvider = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceProvider);

            return container;
        }
    }
}
