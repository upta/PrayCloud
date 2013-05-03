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
        private readonly IMessageHandler messageHandler;
        private readonly IMappingHandler mappingHandler;

        public MessageController( IMessageHandler messageHandler, IMappingHandler mappingHandler )
        {
            this.messageHandler = messageHandler;
            this.mappingHandler = mappingHandler;
        }

        public IEnumerable<MessageDto> Get( string id )
        {
            var messages = this.messageHandler.GetMessagesForUser( id );

            var result = this.mappingHandler.Map<IEnumerable<MessageDto>>( messages );

            return result;
        }

        public void Post( CreateMessageDto message )
        {
            // find peeps to assign it to

            // assign to peeps

            // notify peeps
        }
    }
}
