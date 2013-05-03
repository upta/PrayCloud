using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrayCloud
{
    public interface IUserHandler
    {
        IEnumerable<string> AssignMessageToUsers( Message message, int maxUsers );
        string Create();
        bool Exists( string id );
        User GetUserById( string id );
    }
}