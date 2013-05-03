using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace PrayCloud.Tests.Handlers
{
    [TestClass]
    public class UserHandlerTests
    {
        [TestMethod]
        public void AssignMessageToUsers_DoesntAssignToCreator()
        {
            var users = new List<User>
            {
                new User 
                {
                    Id = "one-message",
                    Messages = new List<Message> { new Message() }
                },
                new User 
                {
                    Id = "two-messages",
                    Messages = new List<Message> { new Message(), new Message() }
                }, 
                new User 
                {
                    Id = "three-messages",
                    Messages = new List<Message> { new Message(), new Message() }
                }
            };

            var message = new Message
            {
                Creator = users.First().Id
            };

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Find<User>() )
                .Returns( users.AsQueryable() );

            var handler = new UserHandler( repo.Object );

            var result = handler.AssignMessageToUsers( message, 2 );

            Assert.IsFalse( result.Any( a => a == message.Creator ) );
        }

        [TestMethod]
        public void AssignMessageToUsers_PrioritizesUsersWithFewerMessages()
        {
            var message = new Message();
            var users = new List<User>
            {
                new User 
                {
                    Id = "one-message",
                    Messages = new List<Message> { new Message() }
                },
                new User 
                {
                    Id = "two-messages",
                    Messages = new List<Message> { new Message(), new Message() }
                }, 
                new User 
                {
                    Id = "three-messages",
                    Messages = new List<Message> { new Message(), new Message() }
                }
            };

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Find<User>() )
                .Returns( users.AsQueryable() );

            var handler = new UserHandler( repo.Object );

            var result = handler.AssignMessageToUsers( message, 2 );

            CollectionAssert.AreEquivalent( users.OrderBy( a => a.Messages.Count )
                                                  .Take( 2 )
                                                  .Select( a => a.Id )
                                                  .ToList(), 
                                            result.ToList() );
        }

        [TestMethod]
        public void AssignMessageToUsers_RespectsMaxUsers()
        {
            var maxUsers = 1;
            var users = new List<User>
            {
                new User 
                {
                    Id = "one-message",
                    Messages = new List<Message> { new Message() }
                },
                new User 
                {
                    Id = "two-messages",
                    Messages = new List<Message> { new Message(), new Message() }
                }, 
                new User 
                {
                    Id = "three-messages",
                    Messages = new List<Message> { new Message(), new Message() }
                }
            };

            var message = new Message
            {
                Creator = users.First().Id
            };

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Find<User>() )
                .Returns( users.AsQueryable() );

            var handler = new UserHandler( repo.Object );

            var result = handler.AssignMessageToUsers( message, maxUsers );

            Assert.AreEqual( maxUsers, result.Count() );
        }

        // need to actually assign the message to the users

        [TestMethod]
        public void Create_ReturnsRepoId()
        {
            var id = "12345";

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Save<User>( It.IsAny<User>() ) )
                .Returns<User>( ( a ) =>
                {
                    a.Id = id;
                    return a;
                } );

            var handler = new UserHandler( repo.Object );

            var result = handler.Create();

            Assert.IsNotNull( result );
            Assert.AreEqual( id, result );
        }

        [TestMethod]
        public void Create_SavesToRepo()
        {
            var id = "12345";

            var repo = new Mock<IRepository>();

            var handler = new UserHandler( repo.Object );

            handler.Create();

            repo.Verify( a => a.Save<User>( It.IsAny<User>() ) );
        }


        [TestMethod]
        public void Exists_ReturnsTrueIfExistsInRepo()
        {
            var id = "12345";

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Find<User>() )
                .Returns( new List<User>() { new User { Id = id } }.AsQueryable() );

            var handler = new UserHandler( repo.Object );

            var result = handler.Exists( id );

            Assert.IsTrue( result );
        }


        [TestMethod]
        public void GetUserById_ThrowsIfNotFound()
        {
            var repo = new Mock<IRepository>();

            var handler = new UserHandler( repo.Object );

            AssertIt.Throws<ArgumentException>( () =>
            {
                handler.GetUserById( null );
            } );
        }

        [TestMethod]
        public void GetUserById_ReturnsIfFound()
        {
            var id = "12345";

            var repo = new Mock<IRepository>();
            repo.Setup( a => a.Find<User>() )
                .Returns( new List<User>() { new User { Id = id } }.AsQueryable() );

            var handler = new UserHandler( repo.Object );

            var result = handler.GetUserById( id );

            Assert.IsNotNull( result );
            Assert.AreEqual( id, result.Id );
        }
    }
}
