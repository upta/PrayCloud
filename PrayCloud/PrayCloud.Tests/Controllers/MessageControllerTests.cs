using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace PrayCloud.Tests.Controllers
{
    [TestClass]
    public class MessageControllerTests
    {
        [TestMethod]
        public void Get_CreatesUserIfDoesntExists()
        {
            var userHandler = new Mock<IUserHandler>();
            userHandler.Setup( a => a.GetUserById( It.IsAny<string>() ) )
                       .Returns( new User() );

            var controller = this.GetController( userHandler: userHandler );

            controller.Get( null );

            userHandler.Verify( a => a.Create() );
        }

        [TestMethod]
        public void Get_GetsUser()
        {
            var id = "12345";

            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.Exists( id ) )
                       .Returns( true );

            userHandler.Setup( a => a.GetUserById( It.IsAny<string>() ) )
                       .Returns( new User() );

            var controller = this.GetController( userHandler: userHandler );

            controller.Get( id );

            userHandler.Verify( a => a.GetUserById( id ) );
        }

        [TestMethod]
        public void Post_CreatesUserIfDoesntExists()
        {
            var userHandler = new Mock<IUserHandler>();
            userHandler.Setup( a => a.GetUserById( It.IsAny<string>() ) )
                       .Returns( new User() );

            var controller = this.GetController( userHandler: userHandler );

            controller.Post( new CreateMessageDto() );

            userHandler.Verify( a => a.Create() );
        }


        private MessageController GetController( Mock<IUserHandler> userHandler = null,
                                                 Mock<IMappingHandler> mappingHandler = null )
        {
            return new MessageController( ( userHandler ?? new Mock<IUserHandler>() ).Object,
                                          ( mappingHandler ?? new Mock<IMappingHandler>() ).Object );
        }
    }
}
