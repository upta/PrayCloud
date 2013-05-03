using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace PrayCloud
{
    public class MemoryRepository : IRepository
    {
        private Dictionary<string, object> data = new Dictionary<string, object>();

        public void Delete<T>( string id )
        {
            this.data.Remove( id );
        }

        public IQueryable<T> Find<T>()
        {
            return (IQueryable<T>) this.data.Values.AsQueryable();
        }

        public T Save<T>( T entity )
        {
            var idProperty = entity.GetType().GetProperty( "Id" );

            if ( idProperty != null )
            {
                if ( idProperty.GetValue( entity ) == null )
                {
                    idProperty.SetValue( entity, Guid.NewGuid().ToString() );
                }
            }

            var id = ( idProperty == null ? Guid.NewGuid().ToString() : idProperty.GetValue( entity ) );

            this.data[ id.ToString() ] = entity;

            return entity;
        }
    }
}