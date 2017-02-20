using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DALLib.Models;
using SQLInfoCollectionService.Configuration;
using SQLInfoCollectionService.Contracts;
using Microsoft.Practices.ServiceLocation;

using System.Threading.Tasks.Schedulers;
using System.Threading.Tasks.Dataflow;
using SQLInfoCollectionService.Scheduler;
using DALLib.Repos;
using SQLInfoCollectionService.Collectors;
using System.Data.SqlClient;
using SQLInfoCollectorService.Scheduler;
using DALLib.Contracts;
using CommonLib;
using Microsoft.Practices.Unity;
using DALLib.EF;
using System.ServiceModel;
using SQLInfoCollectorService.Security;

namespace SQLInfoCollectionService
{
    public partial class CollectionService : ServiceBase,  IWCFContract
    {

        private const string WCFADRESS = "net.tcp://localhost:9999";
        private const string WCFSERVICE = "WCFService";

        private ISLogger logger;
        private IResourceManager resourceManager;
        private IUnitOfWork unitOfWork;
        private IConnectionManager connManager;
        private IEncryptionManager encryptionManager;

        private SQLTaskScheduler sqlTaskScheduler;

        private ServiceHost WCFHost;

        public CollectionService()
        {
            InitializeComponent();



        }




        protected override void OnStart(string[] args)
        {
            

            IUnityContainer unity = DependencyConfig.Initialize();
            logger = ServiceLocator.Current.GetInstance<ISLogger>();
            resourceManager = ServiceLocator.Current.GetInstance<IResourceManager>();
            connManager = ServiceLocator.Current.GetInstance<IConnectionManager>();
            encryptionManager = ServiceLocator.Current.GetInstance<IEncryptionManager>();

            logger.Debug("collection service started");


            unitOfWork = unity.Resolve<IUnitOfWork>(new ParameterOverride("context", new MsSqlMonitorEntities()));


            try
            {
                WCFHost = new ServiceHost(this, new Uri[] { new Uri(WCFADRESS) });
                var behaviour = WCFHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behaviour.InstanceContextMode = InstanceContextMode.Single;

                WCFHost.AddServiceEndpoint(typeof(IWCFContract), new NetTcpBinding(), WCFSERVICE);
                WCFHost.Open();
            }
            catch (Exception e)
            {
                WCFHost = null;
                logger.Error("WCF open error ", e);
            }


            try
            {

                sqlTaskScheduler = new SQLTaskScheduler(logger, resourceManager, unitOfWork, connManager, encryptionManager);
                sqlTaskScheduler.Start();
            }
            catch(Exception e)
            {
                logger.Error("start service error ",e);
            }

          //  


        }



        protected override void OnStop()
        {
            //

            try
            {

               if ( WCFHost!=null )
                      WCFHost.Close();
            }
            catch (Exception e)
            {
                logger.Error("WCF close error ", e);
            }

            try
            {

          
                sqlTaskScheduler.Stop();

                logger.Debug("collection service stopped");
            }
            catch (Exception e)
            {
                logger.Error("stop service error ", e);
            }

            //   sqlTaskScheduler.Stop();


        }



        public void RefreshInstance(int id)
        {
            logger.Debug("refresh was called by WCF");
            sqlTaskScheduler.RefreshInstance(id);
        }
    }




}
