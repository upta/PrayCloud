using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrayCloud
{
    public class CreateMessageDto : MessageDto
    {
        public string Creator { get; set; }
    }
}