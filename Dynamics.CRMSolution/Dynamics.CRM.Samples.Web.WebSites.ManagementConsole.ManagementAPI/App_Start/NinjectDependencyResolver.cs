
using System;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Syntax;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.App_Start
{
   // Provides a Ninject implementation of IDependencyScope
   // which resolves services using the Ninject container.
    /// <summary>
    /// 
    /// </summary>
   public class NinjectDependencyScope : IDependencyScope
   {
      IResolutionRoot resolver;
       /// <summary>
       /// 
       /// </summary>
       /// <param name="resolver"></param>
      public NinjectDependencyScope(IResolutionRoot resolver)
      {
         this.resolver = resolver;
      }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="serviceType"></param>
       /// <returns></returns>
      public object GetService(Type serviceType)
      {
         if (resolver == null)
            throw new ObjectDisposedException("this", "This scope has been disposed");

         return resolver.TryGet(serviceType);
      }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="serviceType"></param>
       /// <returns></returns>
      public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
      {
         if (resolver == null)
            throw new ObjectDisposedException("this", "This scope has been disposed");

         return resolver.GetAll(serviceType);
      }

       /// <summary>
       /// 
       /// </summary>
      public void Dispose()
      {
         IDisposable disposable = resolver as IDisposable;
         if (disposable != null)
            disposable.Dispose();

         resolver = null;
      }
   }

   // This class is the resolver, but it is also the global scope
   // so we derive from NinjectScope.
    /// <summary>
    /// 
    /// </summary>
   public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
   {
      IKernel kernel;

       /// <summary>
       /// 
       /// </summary>
       /// <param name="kernel"></param>
      public NinjectDependencyResolver(IKernel kernel) : base(kernel)
      {
         this.kernel = kernel;
      }

       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
      public IDependencyScope BeginScope()
      {
         return new NinjectDependencyScope(kernel.BeginBlock());
      }
   }
}