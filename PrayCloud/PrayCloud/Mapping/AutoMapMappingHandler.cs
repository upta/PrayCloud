using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace PrayCloud
{
    public class AutoMapMappingHandler : IMappingHandler
    {
        public AutoMapMappingHandler()
        {
            Mapper.Reset();

            Mapper.CreateMap<Message, MessageDto>();
            Mapper.CreateMap<CreateMessageDto, Message>();
        }

        public U Map<U>( object source )
        {
            return Mapper.Map<U>( source );
        }
    }
}