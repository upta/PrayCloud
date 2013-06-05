using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Ninject;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Configuration;

namespace PrayCloud
{
    public class IocConfig
    {
        static public StandardKernel Register( HttpConfiguration config )
        {
            var kernel = new StandardKernel();

            try
            {
                kernel.Bind<IRepository>().ToConstant( new MongoRepository( ConfigurationManager.ConnectionStrings[ "Mongo" ].ConnectionString, "onlypray" ) );
            }
            catch ( Exception ex ) // probably a bad/missing connection string for the mongodb, just use an in-memory store to get running instead
            {
                kernel.Bind<IRepository>().ToConstant( new MemoryRepository() );
            }

            kernel.Bind<IUserHandler>().To<UserHandler>();
            kernel.Bind<IMessageHandler>().To<MessageHandler>();
            kernel.Bind<IMappingHandler>().To<AutoMapMappingHandler>();

            DependencyResolver.SetResolver( new NinjectMVCDependencyResolver( kernel ) );
            config.DependencyResolver = new NinjectHttpDependencyResolver( kernel );

            return kernel;
        }
    }
}