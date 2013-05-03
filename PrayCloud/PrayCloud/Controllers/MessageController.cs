using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PrayCloud
{
    public class MessageController : ApiController
    {
        private readonly IUserHandler userHandler;
        private readonly IMappingHandler mappingHandler;

        public MessageController( IUserHandler userHandler, IMappingHandler mappingHandler )
        {
            this.userHandler = userHandler;
            this.mappingHandler = mappingHandler;
        }

        public IEnumerable<MessageDto> Get( string id )
        {
            if ( !this.userHandler.Exists( id ) )
            {
                id = this.userHandler.Create();
            }

            var user = this.userHandler.GetUserById( id );

            var result = this.mappingHandler.Map<IEnumerable<MessageDto>>( user.Messages );

            return result;
        }

        public void Post( CreateMessageDto message )
        {
            if ( !this.userHandler.Exists( message.Creator ) )
            {
                message.Creator = this.userHandler.Create();
            }

            // find peeps to assign it to

            // assign to peeps

            // notify peeps
        }
    }
}
