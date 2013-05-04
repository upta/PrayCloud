using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace PrayCloud.Tests.Handlers
{
    [TestClass]
    public class MessageHandlerTests
    {
        [TestMethod]
        public void GetMessagesForUser_PullsMessagesFromRepo()
        {
            var id = "12345";
            var messages = new List<Message>
            {
                new Message
                {
                   Users = new List<string> { "not our user", id }
                },
                new Message
                {
                   Users = new List<string> { "not our user" }
                },
                new Message
                {
                   Users = new List<string> { id }
                }
            };

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Find<Message>() )
                .Returns( messages.AsQueryable() );

            var handler = new MessageHandler( repo.Object );

            var result = handler.GetMessagesForUser( id );

            CollectionAssert.AreEquivalent( messages.Where( a => a.Users.Any( b => b == id ) ).ToList(),
                                            result.ToList() );
        }
    }
}
