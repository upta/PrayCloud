using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrayCloud
{
    public interface IUserHandler
    {
        string Create();
        bool Exists( string id );
        User GetUserById( string id );
    }
}