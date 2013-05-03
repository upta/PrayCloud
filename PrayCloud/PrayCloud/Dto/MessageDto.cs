using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrayCloud
{
    public class MessageDto
    {
        public string Body { get; set; }

        public DateTime Created { get; set; }

        public string Id { get; set; }
    }
}