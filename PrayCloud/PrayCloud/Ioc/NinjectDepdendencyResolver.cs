using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Syntax;

namespace Ninject
{
    public class NinjectMVCDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectMVCDependencyResolver( IKernel kernel )
        {
            this.kernel = kernel;
        }

        public object GetService( Type serviceType )
        {
            return this.kernel.TryGet( serviceType );
        }

        public IEnumerable<object> GetServices( Type serviceType )
        {
            return this.kernel.GetAll( serviceType );
        }
    }

    public class NinjectHttpDependencyScope : System.Web.Http.Dependencies.IDependencyScope
    {
        protected IResolutionRoot resolutionRoot;

        public NinjectHttpDependencyScope( IResolutionRoot kernel )
        {
            resolutionRoot = kernel;
        }

        public object GetService( Type serviceType )
        {
            IRequest request = resolutionRoot.CreateRequest( serviceType, null, new Parameter[ 0 ], true, true );
            return resolutionRoot.Resolve( request ).SingleOrDefault();
        }

        public IEnumerable<object> GetServices( Type serviceType )
        {
            IRequest request = resolutionRoot.CreateRequest( serviceType, null, new Parameter[ 0 ], true, true );
            return resolutionRoot.Resolve( request ).ToList();
        }

        public void Dispose()
        {
            IDisposable disposable = (IDisposable) resolutionRoot;
            if ( disposable != null ) disposable.Dispose();
            resolutionRoot = null;
        }
    }

    public class NinjectHttpDependencyResolver : NinjectHttpDependencyScope, System.Web.Http.Dependencies.IDependencyResolver
    {
        private IKernel _kernel;
        public NinjectHttpDependencyResolver( IKernel kernel )
            : base( kernel )
        {
            _kernel = kernel;
        }
        public System.Web.Http.Dependencies.IDependencyScope BeginScope()
        {
            return new NinjectHttpDependencyScope( _kernel.BeginBlock() );
        }
    }
}