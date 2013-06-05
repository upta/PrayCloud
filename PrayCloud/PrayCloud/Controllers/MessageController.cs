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
        private readonly IMessageHandler messageHandler;
        private readonly IMappingHandler mappingHandler;

        public MessageController( IUserHandler userHandler, IMessageHandler messageHandler, IMappingHandler mappingHandler )
        {
            this.userHandler = userHandler;
            this.messageHandler = messageHandler;
            this.mappingHandler = mappingHandler;
        }

        public IEnumerable<MessageDto> Get( string id = null )
        {
            if ( !this.userHandler.Exists( id ) )
            {
                id = this.userHandler.Create();
            }

            var messages = this.messageHandler.GetMessagesForUser( id );

            var result = this.mappingHandler.Map<IEnumerable<MessageDto>>( messages );

            return result;
        }

        public void Post( CreateMessageDto dto )
        {
            if ( !this.userHandler.Exists( dto.Creator ) )
            {
                dto.Creator = this.userHandler.Create();
            }

            var message = this.mappingHandler.Map<Message>( dto );

            this.messageHandler.DispatchMessage( message, this.messageHandler.CalculateMaxDispatchUsers() );

            // notify peeps
        }
    }
}
