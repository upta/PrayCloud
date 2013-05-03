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
            var messages = this.repository.Find<Message>()
                                          .Where( a => a.Users.Any( b => b == id ) );

            if ( !messages.Any() )
            {
                messages = this.repository.Find<Message>()
                                          .Where( a => a.Creator != id )
                                          .OrderByDescending( a => a.Created )
                                          .Take( 3 );

                foreach ( var message in messages )
                {
                    message.Users.Add( id );
                    this.repository.Save<Message>( message );
                }
            }

            return messages;
        }

    }
}