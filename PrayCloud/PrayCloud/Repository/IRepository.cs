using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrayCloud
{
    public interface IRepository
    {
        void Delete<T>( string id );
        IQueryable<T> Find<T>();
        T Save<T>( T entity );
    }
}