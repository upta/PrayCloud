using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrayCloud
{
    public interface IRepository
    {
        void Delete<T>( string id ) where T : class;
        IQueryable<T> Find<T>() where T : class;
        T Save<T>( T entity ) where T : class;
    }
}