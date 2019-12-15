[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Pavliks.WAM.ManagementConsole.ManagementAPI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Pavliks.WAM.ManagementConsole.ManagementAPI.App_Start.NinjectWebCommon), "Stop")]

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.App_Start
{
    using System;
    using System.Web;
    using Interfaces = Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
    using Implementation = Pavliks.WAM.ManagementConsole.Infrastructure.Implementation;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Reflection;
    using System.Web.Http;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.Bind<Interfaces.ISalesOrderRepository>().To<Implementation.SalesOrderCRM>();
            kernel.Bind<Interfaces.IRegistrationRepository>().To<Implementation.RegistrationCRM>();
            kernel.Bind<Interfaces.IEventRepository>().To<Implementation.EventCRM>();

            kernel.Bind<Interfaces.IWaitlistRepository>().To<Implementation.WailistCRM>();
            kernel.Bind<Interfaces.IProductRepository>().To<Implementation.ProductCRM>();
            kernel.Bind<Interfaces.ISessionRepository>().To<Implementation.SessionCRM>();
            kernel.Bind<Interfaces.IReservationRepository>().To<Implementation.ReservationCRM>();
            kernel.Bind<Interfaces.IContactRepository>().To<Implementation.ContactCRM>();
            kernel.Bind<Interfaces.ISalesOrderItemRepository>().To<Implementation.SalesOrderItemCRM>();
            kernel.Bind<Interfaces.IOrderManagementItemRepository>().To<Implementation.OrderManagementItemCRM>();
            kernel.Bind<Interfaces.IEmailRepository>().To<Implementation.EmailCRM>();
            kernel.Bind<Interfaces.IOrderTransactionRepository>().To<Implementation.OrderTransactionCRM>();
            kernel.Bind<Interfaces.IConfigurationRepository>().To<Implementation.ConfigurationCRM>();
            kernel.Bind<Interfaces.ISMSMessageRepository>().To<Implementation.SMSMessageCRM>();
            kernel.Bind<Interfaces.IOrderCoursesRepository>().To<Implementation.OrderCoursesCRM>();

        }
    }
}
