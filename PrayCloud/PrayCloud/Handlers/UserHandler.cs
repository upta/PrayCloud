using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrayCloud
{
    public class UserHandler : IUserHandler
    {
        private readonly IRepository repository;

        public UserHandler( IRepository repository )
        {
            this.repository = repository;
        }


        public IEnumerable<string> AssignMessageToUsers( Message message, int maxUsers )
        {
            var users = this.repository.Find<User>()
                                       .Where( a => a.Id != message.Creator )
                                       .OrderBy( a => a.Messages.Count )
                                       .Take( maxUsers );

            return users.Select( a => a.Id );
        }

        public string Create()
        {
            var user = new User();

            this.repository.Save<User>( user );

            return user.Id;
        }

        public bool Exists( string id )
        {
            return this.repository.Find<User>().Any( a => a.Id == id );
        }

        public User GetUserById( string id )
        {
            var user = this.repository.Find<User>().FirstOrDefault( a => a.Id == id );

            if ( user == null )
            {
                throw new ArgumentException( "User '{0}' wasn't found", id ?? "null" );
            }

            return user;
        }
    }
}