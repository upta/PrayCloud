using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayCloud
{
    public interface IMappingHandler
    {
        U Map<U>( object entity );
    }
}
