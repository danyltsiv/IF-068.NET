using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLib;
using DALLib.Contracts;
using DALLib;
using DALLib.EF;
using Microsoft.Practices.ServiceLocation;
using DALLib.Models;
using SQLInfoCollectorService.Security;
using System.Web.Http.Dispatcher;

namespace ASPNETAPP.Unity
{
    public class DependencyConfig
    {
        public static IUnityContainer Initialize()
        {

            IUnityContainer container = new UnityContainer();

            container.RegisterType<IEncryption, Encryption>();
            container.RegisterType<IEncryptionManager, EncryptionManager>();
            container.RegisterType<ISLogger, SLogger>();

            container.RegisterInstance<IHttpControllerActivator>(new ServiceActivator(container));


            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IUnitOfWork, UnitOfWork>(new InjectionConstructor(
                            typeof(MsSqlMonitorEntities),
                            new ApplicationUserManager(new UserStore(new MsSqlMonitorEntities())),
                            new ApplicationRoleManager(new RoleStore(new MsSqlMonitorEntities()))
                            ));
            //  container.RegisterType<IInstanceDataCollector, InstanceDataCollector>();


            UnityServiceLocator serviceProvider = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceProvider);

            return container;
        }
    }
}
