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

            var handler = new UserHandler( repo.Object, new Mock<IMessageHandler>().Object );

            var result = handler.Create();

            Assert.IsNotNull( result );
            Assert.AreEqual( id, result );
        }

        [TestMethod]
        public void Create_EnsuresUserGetsStartingMessages()
        {
            var repo = new Mock<IRepository>();

            var messageHandler = new Mock<IMessageHandler>();

            var handler = new UserHandler( repo.Object, messageHandler.Object );

            handler.Create();

            messageHandler.Verify( a => a.EnsureUserHasMessages( It.IsAny<string>() ) );
        }

        [TestMethod]
        public void Create_SavesToRepo()
        {
            var id = "12345";

            var repo = new Mock<IRepository>();

            var handler = new UserHandler( repo.Object, new Mock<IMessageHandler>().Object );

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

            var handler = new UserHandler( repo.Object, new Mock<IMessageHandler>().Object );

            var result = handler.Exists( id );

            Assert.IsTrue( result );
        }


        [TestMethod]
        public void GetUserById_ThrowsIfNotFound()
        {
            var repo = new Mock<IRepository>();

            var handler = new UserHandler( repo.Object, new Mock<IMessageHandler>().Object );

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

            var handler = new UserHandler( repo.Object, new Mock<IMessageHandler>().Object );

            var result = handler.GetUserById( id );

            Assert.IsNotNull( result );
            Assert.AreEqual( id, result.Id );
        }
    }
}
