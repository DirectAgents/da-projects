﻿using System;
using System.Web.Mvc;
using System.Web.Routing;
using EomTool.Domain.Abstract;
using EomTool.Domain.Concrete;
using EomTool.Domain.Entities;
using Ninject;

namespace EomToolWeb.Infrastructure
{
    public partial class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel kernel;

        public NinjectControllerFactory()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)kernel.Get(controllerType);
        }

        private void AddBindings()
        {
            kernel.Bind<IEomEntitiesConfig>().To<EomEntitiesConfig>();
            kernel.Bind<IAffiliateRepository>().To<AffiliateRepository>();
            kernel.Bind<IMainRepository>().To<MainRepository>();
            kernel.Bind<ISecurityRepository>().To<SecurityRepository>();
            kernel.Bind<IBatchRepository>().To<BatchRepository>();
            kernel.Bind<IDAMain1Repository>().To<DAMain1Repository>();
            kernel.Bind<IPaymentBatchRepository>().To<PaymentBatchRepository>();
            SetupOther();            
        }
    }
}