using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrayCloud
{
    public class MessageHandler : IMessageHandler 
    {
        private readonly IRepository repository;

        public MessageHandler( IRepository repository )
        {
            this.repository = repository;
        }

        public IEnumerable<Message> GetMessagesForUser( string id )
        {
            return this.repository.Find<Message>()
                                  .Where( a => a.Users.Any( b => b == id ) );
        }

        public void DispatchMessage( Message message, int maxUsers )
        {

        }
    }
}