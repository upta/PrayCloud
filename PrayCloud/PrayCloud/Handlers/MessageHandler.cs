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


        public int CalculateMaxDispatchUsers()
        {
            return 7; // very scientific... :) stub until we can make an educated guess on how much output volume we should generate
        }

        public void DispatchMessage( Message message, int maxUsers )
        {
            var users = this.repository.Find<User>()
                                       .Where( a => a.Id != message.Creator )
                                       .OrderBy( a => a.LastAssigned )
                                       .Take( maxUsers );

            message.Users = users.Select( a => a.Id ).ToList();
            message.Created = DateTime.UtcNow;
            this.repository.Save<Message>( message );

            foreach ( var user in users )
            {
                user.LastAssigned = DateTime.UtcNow;
                this.repository.Save<User>( user );
            }
        }

        public void EnsureUserHasMessages( string userId )
        {
            if ( !this.GetMessagesForUser( userId ).Any() )
            {
                var latestMessages = this.repository.Find<Message>()
                                                    .Where( a => a.Creator != userId )
                                                    .OrderByDescending( a => a.Created )
                                                    .Take( 3 );

                foreach ( var message in latestMessages )
                {
                    message.Users.Add( userId );

                    this.repository.Save<Message>( message );
                }
            }
        }

        public IEnumerable<Message> GetMessagesForUser( string id )
        {
            var result = this.repository.Find<Message>()
                                        .Where( a => a.Users.Contains( id ) );

            return result;
        }
    }
}