using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration.Install;

using Microsoft.Practices.ServiceLocation;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.Configuration;
using DALLib.Contracts;
using Microsoft.Practices.Unity;
using DALLib.EF;
using CommonLib;

namespace SQLInfoCollectionService
{
    static class Program
    {

        static string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        private static bool IsInstalled()
        {
            using (ServiceController controller =
                new ServiceController(name))
            {
                try
                {
                    ServiceControllerStatus status = controller.Status;
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }

        private static bool IsRunning()
        {
            using (ServiceController controller =
                new ServiceController(name))
            {
                if (!IsInstalled()) return false;
                return (controller.Status == ServiceControllerStatus.Running);
            }
        }

        private static AssemblyInstaller GetInstaller(ISLogger logger)
        {
            AssemblyInstaller installer = new AssemblyInstaller( typeof(CollectionService).Assembly, null);
            installer.UseNewContext = true;
            return installer;
        }

        private static void InstallService(ISLogger logger)
        {
            if (IsInstalled())
            {
                Console.WriteLine("Try to install collection service but, service already installed! ");
                logger.Debug("Try to install collection service but, service already installed! ");
                return;
            }

            logger.Debug("Install collection service... ");

            AssemblyInstaller installer = GetInstaller(logger);
            IDictionary state = new Hashtable();

            try
            {

                installer.Install(state);
                installer.Commit(state);

            }
            catch (Exception e)
            {

                Console.WriteLine("Install CollectionService error " + e.Message);
                logger.Error("Install CollectionService error " + e.Message);
                logger.Error(e.ToString());
            }
            finally
            {
                try { installer.Rollback(state); } catch { };
            }
        }

        private static void UninstallService(ISLogger logger)
        {
            if (!IsInstalled())
            {
                Console.WriteLine("CollectionService already uninstalled! ");
                logger.Debug("CollectionService already uninstalled! ");
                return;
            }



            try
            {
                using (AssemblyInstaller installer = GetInstaller(logger))
                {
                    IDictionary state = new Hashtable();
                    try
                    {
                        installer.Uninstall(state);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Uninstall CollecionService error " + e.Message);
                        logger.Error("Uninstall CollecionService error " + e.Message);
                        logger.Error(e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Uninstall CollecionService error " + e.Message);
                logger.Error("Uninstall CollecionService error " + e.Message);
                logger.Error(e.ToString());
            }
        }

        private static void StartService(ISLogger logger)
        {
            if (!IsInstalled())
            {
                logger.Debug("CollectionService already started! ");
                Console.WriteLine("CollectionService already started! ");
                return;
            }

            logger.Debug("Starting  collection service.... ");
            Console.WriteLine("Starting  collection service.... ");

            using (ServiceController controller =
                new ServiceController(name))
            {
                try
                {
                    if (controller.Status != ServiceControllerStatus.Running)
                    {
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Start CollectionService() error " + e.Message);
                    logger.Error("Start CollectionService() error " + e.Message);
                    logger.Error(e.ToString());
                }
            }
        }

        private static void StopService(ISLogger logger)
        {
            if (!IsInstalled())
            {
                logger.Debug("try stop  collection service, but it is not installed! ");
                Console.WriteLine("try stop  collection service, but it is not installed! ");
                return;
            }

            logger.Debug("Stopping  collection service... ");
            Console.WriteLine("Stopping  collection service... ");


            using (ServiceController controller =
                new ServiceController(name))
            {
                try
                {
                    if (controller.Status != ServiceControllerStatus.Stopped)
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped,
                             TimeSpan.FromSeconds(10));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Stop CollectionService error " + e.Message);
                    logger.Error("Stop CollectionService error " + e.Message);
                    logger.Error(e.ToString());
                }
            }
        }




        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new CollectionService()
            };
            ServiceBase.Run(ServicesToRun);
     /*
            ISLogger logger = ServiceLocator.Current.GetInstance<ISLogger>();

            if (args.Length == 0) //no arguments
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new CollectionService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            else          

            {
                switch (args[0])
                {
                    case "--install":
                        InstallService(logger);
                        StartService(logger);
                        break;
                    case "--uninstall":
                        StopService(logger);
                        UninstallService(logger);
                        break;
                    default:
                        Console.WriteLine("collection service wrong  run argument ");
                        logger.Debug("collection service wrong  run argument ");
                        break;
                }
            }
*/

        }///Main



    }//Program


}
