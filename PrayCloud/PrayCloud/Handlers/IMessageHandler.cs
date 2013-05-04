using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrayCloud
{
    public interface IMessageHandler
    {
        IEnumerable<Message> GetMessagesForUser( string id );
        void DispatchMessage( Message message, int maxUsers );
    }
}