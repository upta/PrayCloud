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
        public void GetMessagesForUser_PullsUserMesssagesFromRepo()
        {
            var id = "12345";

            var messages = new List<Message>
            {
                new Message
                {
                    Id = "1",
                    Body = "body",
                    Created = DateTime.MaxValue,
                    Users = new List<string> { id }
                },
                new Message
                {
                    Id = "2",
                    Body = "body2",
                    Created = DateTime.MinValue,
                    Users = new List<string> { id, "some-value" }
                },
            };

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Find<Message>() )
                .Returns( messages.AsQueryable() );

            var handler = new MessageHandler( repo.Object );

            var result = handler.GetMessagesForUser( id );

            Assert.IsNotNull( result );
            CollectionAssert.AreEquivalent( messages, result.ToList() );
        }

        [TestMethod]
        public void GetMessagesForUser_AssignsUserRecentMessagesIfTheyHaveNone()
        {
            var id = "12345";

            var messages = new List<Message>
            {
                new Message
                {
                    Id = "1",
                    Body = "body",
                    Creator = id,
                    Created = DateTime.MaxValue,
                    Users = new List<string> { "not-user" }
                },
                new Message
                {
                    Id = "2",
                    Body = "body2",
                    Created = DateTime.MinValue,
                    Users = new List<string> { "not-user", "some-value" }
                },
            };

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Find<Message>() )
                .Returns( messages.AsQueryable() );

            var handler = new MessageHandler( repo.Object );

            var result = handler.GetMessagesForUser( id );

            Assert.IsNotNull( result );
            CollectionAssert.AreEquivalent( messages.Where( a => a.Creator != id ).ToList(), result.ToList() );
            repo.Verify( a => a.Save<Message>( It.IsAny<Message>() ), Times.Exactly( messages.Count( a => a.Creator != id ) ) );
        }
    }
}
