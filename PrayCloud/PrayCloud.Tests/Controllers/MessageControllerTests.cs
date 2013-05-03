using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace PrayCloud.Tests.Controllers
{
    [TestClass]
    public class MessageControllerTests
    {
        [TestMethod]
        public void Get_GetMessagesForUser()
        {
            var id = "12345";

            var messageHandler = new Mock<IMessageHandler>();

            var controller = this.GetController( messageHandler );

            controller.Get( id );

            messageHandler.Verify( a => a.GetMessagesForUser( id ) );
        }


        private MessageController GetController( Mock<IMessageHandler> messageHandler = null, 
                                                 Mock<IMappingHandler> mappingHandler = null )
        {
            return new MessageController( ( messageHandler ?? new Mock<IMessageHandler>() ).Object,
                                          ( mappingHandler ?? new Mock<IMappingHandler>() ).Object );
        }
    }
}
