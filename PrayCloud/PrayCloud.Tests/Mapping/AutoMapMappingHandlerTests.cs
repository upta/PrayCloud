using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PrayCloud.Tests.Mapping
{
    [TestClass]
    public class AutoMapMappingHandlerTests
    {
        [TestMethod]
        public void Map_MessageToMessageDto()
        {
            var message = new Message
            {
                Body = "body",
                Created = DateTime.MaxValue,
                Creator = "creator",
                Id = "12345",
                Users = new List<string>() { "user", "ids" }
            };

            IEnumerable<Message> messages = new List<Message>() { message };

            var handler = new AutoMapMappingHandler();

            var result = handler.Map<MessageDto>( message );

            Assert.AreEqual( message.Body, result.Body );
            Assert.AreEqual( message.Created, result.Created );
            Assert.AreEqual( message.Id, result.Id );

            var resultCollection = handler.Map<IEnumerable<MessageDto>>( messages );

            Assert.IsTrue( resultCollection.Any( a => a.Body == result.Body && 
                                                 a.Created == result.Created && 
                                                 a.Id == result.Id ) );
        }
    }
}
