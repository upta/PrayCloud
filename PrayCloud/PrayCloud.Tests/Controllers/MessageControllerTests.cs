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
        public void Get_GetsMessagesForUser()
        {
            var id = "12345";

            var userHandler = new Mock<IUserHandler>();

            userHandler.Setup( a => a.Exists( id ) )
                       .Returns( true );

            var messageHandler = new Mock<IMessageHandler>();

            var controller = this.GetController( userHandler: userHandler,
                                                 messageHandler: messageHandler );

            controller.Get( id );

            messageHandler.Verify( a => a.GetMessagesForUser( id ) );
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

        [TestMethod]
        public void Post_DispatchesMessage()
        {
            var id = "12345";
            var dto = new CreateMessageDto
            {
                Creator = id
            };

            var messageHandler = new Mock<IMessageHandler>();

            var controller = this.GetController( messageHandler: messageHandler );

            controller.Post( dto );

            messageHandler.Verify( a => a.DispatchMessage( It.IsAny<Message>(), It.IsAny<int>() ) );
        }


        private MessageController GetController( Mock<IUserHandler> userHandler = null,
                                                 Mock<IMessageHandler> messageHandler = null,
                                                 Mock<IMappingHandler> mappingHandler = null )
        {
            return new MessageController( ( userHandler ?? new Mock<IUserHandler>() ).Object,
                                          ( messageHandler ?? new Mock<IMessageHandler>() ).Object,
                                          ( mappingHandler ?? new Mock<IMappingHandler>() ).Object );
        }
    }
}
