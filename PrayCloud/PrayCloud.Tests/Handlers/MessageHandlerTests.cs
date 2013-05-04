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
        public void CalculateMaxDispatchUsers_Returns7AsStub()
        {
            var handler = new MessageHandler( new Mock<IRepository>().Object );

            var result = handler.CalculateMaxDispatchUsers();

            Assert.AreEqual( 7, result );
        }


        [TestMethod]
        public void DispatchMessage_AssignsUsers_WithEarliestLastAssigned_WhoArentCreator()
        {
            var id = "12345";
            var message = new Message
            {
                Creator = id,
                Users = new List<string>()
            };

            var users =new List<User> 
            { 
                new User
                { 
                    Id = id,
                    LastAssigned = DateTime.MinValue
                },
                new User
                { 
                    Id = "newer",
                    LastAssigned = DateTime.MinValue.AddDays( 1 )
                },
                new User
                { 
                    Id = "older",
                    LastAssigned = DateTime.MinValue.AddDays( 1 )
                }
            };

            var expectedUsers = users.Where( a => a.Id == "newer" );

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Find<User>() ).Returns( users.AsQueryable() );

            var handler = new MessageHandler( repo.Object );

            handler.DispatchMessage( message, 1 );

            CollectionAssert.AreEquivalent( expectedUsers.Select( a => a.Id ).ToList(), message.Users );
            repo.Verify( a => a.Save<Message>( message ) );
        }

        [TestMethod]
        public void DispatchMessage_UpdatesLastModified_OfAssignedUsers()
        {
            var id = "12345";
            var message = new Message
            {
                Creator = id,
                Users = new List<string>()
            };

            var users = new List<User> 
            { 
                new User
                { 
                    Id = id,
                    LastAssigned = DateTime.MinValue
                },
                new User
                { 
                    Id = "newer",
                    LastAssigned = DateTime.MinValue.AddDays( 1 )
                },
                new User
                { 
                    Id = "older",
                    LastAssigned = DateTime.MinValue.AddDays( 1 )
                }
            };

            var expectedUser = users.First( a => a.Id == "newer" );
            var originalLastAssigned = expectedUser.LastAssigned;

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Find<User>() ).Returns( users.AsQueryable() );

            var handler = new MessageHandler( repo.Object );

            handler.DispatchMessage( message, 1 );

            Assert.IsTrue( originalLastAssigned < expectedUser.LastAssigned );
            repo.Verify( a => a.Save<User>( expectedUser ) );
        }


        [TestMethod]
        public void EnsureUserHasMessages_DoesntPullIfExisting()
        {
            var id = "12345";
            var messages = new List<Message>
            {
                new Message
                {
                    Users = new List<string>() { id }
                }
            };

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Find<Message>() )
                .Returns( messages.AsQueryable() );

            var handler = new MessageHandler( repo.Object );

            handler.EnsureUserHasMessages( id );

            CollectionAssert.AreEquivalent( messages, handler.GetMessagesForUser( id ).ToList() );
        }

        [TestMethod]
        public void EnsureUserHasMessages_AssignsLatestMessages_ThatUserDidntCreate_IfNoneAssigned()
        {
            var id = "12345";
            var messages = new List<Message>
            {
                new Message
                {
                    Created = DateTime.MaxValue,
                    Users= new List<string>()
                },
                new Message
                {
                    Creator = id,
                    Created = DateTime.MaxValue.AddDays( -1 ),
                    Users= new List<string>()
                },
                new Message
                {
                    Created = DateTime.MaxValue.AddDays( -2 ),
                    Users= new List<string>()
                }
            };

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Find<Message>() )
                .Returns( messages.AsQueryable() );

            var handler = new MessageHandler( repo.Object );

            handler.EnsureUserHasMessages( id );

            CollectionAssert.AreEquivalent( messages.Where( a => a.Creator != id ).ToList(), handler.GetMessagesForUser( id ).ToList() );
            repo.Verify( a => a.Save<Message>( It.IsAny<Message>() ), Times.Exactly( messages.Count( a => a.Creator != id ) ) );
        }


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
