using CommonLib;
using DALLib.Contracts;
using DALLib.EF;
using DALLib.Models;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SQLInfoCollectionService.Configuration;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectorService.Scheduler;
using SQLInfoCollectorService.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForTesting

{

    public class WCFTEST : IWCFContract
    {
        public WCFTEST()
        {
            Console.WriteLine("WCFTEST CONSTRUCTOR");
        }

        public void RefreshInstance(int id)
        {
            Console.WriteLine("RefreshInstance"+id.ToString());
        }
    }


    class testService 
    {
        private ISLogger logger;
        private IResourceManager resourceManager;
        private IUnitOfWork unitOfWork;
        private IEncryptionManager encryptionManager;

        private SQLTaskScheduler sqlTaskScheduler;
        private ServiceHost WCFHost;

        public testService()
        {
            IUnityContainer unity = DependencyConfig.Initialize();

            logger = ServiceLocator.Current.GetInstance<ISLogger>();
            encryptionManager = ServiceLocator.Current.GetInstance<IEncryptionManager>();
            resourceManager = ServiceLocator.Current.GetInstance<IResourceManager>();
            unitOfWork = unity.Resolve<IUnitOfWork>(new ParameterOverride("context", new MsSqlMonitorEntities()));
            IConnectionManager connManager = ServiceLocator.Current.GetInstance<IConnectionManager>();

            sqlTaskScheduler = new SQLTaskScheduler(logger, resourceManager, unitOfWork, connManager, encryptionManager);
   
        }



        public void start()
        {

            

            try
            {
                WCFHost = new ServiceHost(typeof(WCFTEST),new Uri[] { new Uri("net.tcp://localhost:9999") });
                WCFHost.AddServiceEndpoint(typeof(IWCFContract),new NetTcpBinding(), "WCFService");
                WCFHost.Open();
            }
            catch (Exception e)
            {
                WCFHost = null;
                logger.Error("WCF open error ", e);
            }
            sqlTaskScheduler.Start();
        }

        public void stop()
        {
            try
            {

                if (WCFHost != null)
                    WCFHost.Close();
            }
            catch (Exception e)
            {
                logger.Error("WCF close error ", e);
            }

            sqlTaskScheduler.Stop();
        }

        public void RefreshInstance(int id)
        {
            throw new NotImplementedException();
        }
    }
    class Program
    {





        static void Main(string[] args)
        {

            //addDataForTesting();



            testService t = new testService();
            t.start();



            Console.WriteLine("Press any key to stop....");
            Console.ReadKey();


            t.stop();

        }


        static void  addDataForTesting()
        {
        
      
            List<BrowsableInstance> list = CommonLib.BrowsableInstance.GetInstances();
            MsSqlMonitorEntities db = new MsSqlMonitorEntities();
            //db.SaveChanges();

            /*
            User user;
            if (db.Users.LongCount<User>() == 0)
            {
                user = new User();
                user.Login = "admin";
                user.Password = "admin";
                user.Role = UserRole.Admin;
                db.Users.Add(user);
                db.SaveChanges();

                user = new User();
                user.Login = "user";
                user.Password = "user";
                user.Role = UserRole.User;
                db.Users.Add(user);
                db.SaveChanges();


            } else
            {
                user = db.Users.First<User>();
            }
            */

            foreach (var browsableInstance in list)
            {
                if (db.Instances.Where<Instance>(x => x.InstanceName.Equals(browsableInstance.InstanceName)).LongCount() != 0) continue;

                Instance newInst = new Instance();
                newInst.Authentication = AuthenticationType.Sql;
                newInst.InstanceName = browsableInstance.InstanceName;
                newInst.ServerName = browsableInstance.ServerName;
                newInst.Login = "sa";
                newInst.Password = "awsaws";
                newInst.IsDeleted = false;

                db.Instances.Add(newInst);
              //  db.
                ///////////////////////////////////////////////////

                /*
                Assign assign = new Assign();
                assign.User = user;
                assign.UserId = user.Id;
                assign.Instance = newInst;
                assign.InstanceId = newInst.Id;

                db.Assigns.Add(assign);
                */

                db.SaveChanges();
                //////////////////////////////////////////////

                /*
                InstanceUpdateJob updateJob = new InstanceUpdateJob();
                updateJob.Instance = newInst;
                updateJob.InstanceId = newInst.Id;

                JobType  jobType = db.JobTypes.Where<JobType>(i => i.Type == JobType.UpdateInfoType.Full).First<JobType>();

                updateJob.JobType = jobType;
                updateJob.JobTypeId = jobType.Type;


                db.InstanceUpdateJobs.Add(updateJob);


                db.SaveChanges();
              
                //////////////////////////////////////////////

                InstanceUpdateJob updateJob2 = new InstanceUpdateJob();
                updateJob2.Instance = newInst;
                updateJob2.InstanceId = newInst.Id;

                JobType jobType2 = db.JobTypes.Where<JobType>(i => i.Type == JobType.UpdateInfoType.CheckStatus).First<JobType>();

                updateJob2.JobType = jobType2;
                updateJob2.JobTypeId = jobType.Type;


                db.InstanceUpdateJobs.Add(updateJob2);

              */

                db.SaveChanges();


                //////////////////////////////////////////////

            }
           // db.SaveChanges();



 



          //  db.SaveChanges();


        }




    }
}
