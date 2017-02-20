using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;

namespace ASPNETAPP.Unity
{
    public class ServiceActivator : IHttpControllerActivator
    {
        private IUnityContainer container;

        public ServiceActivator(IUnityContainer container) { this.container = container; }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            //var controller = Factory.GetInstance(controllerType) as IHttpController;
            return (IHttpController) container.Resolve(controllerType);
           // return null;
        }
    }




}