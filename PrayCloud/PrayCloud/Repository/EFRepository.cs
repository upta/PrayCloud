using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace PrayCloud
{
    public class EFRepository : IRepository
    {
        private readonly DbContext data;

        public EFRepository( DbContext data )
        {
            this.data = data;
        }

        public void Delete<T>( string id ) where T : class
        {
            var entity = this.data.Set<T>().Find( id );

            if ( entity != null )
            {
                this.data.Set<T>().Remove( entity );
            }
        }

        public IQueryable<T> Find<T>() where T : class
        {
            return (IQueryable<T>) this.data.Set<T>().AsQueryable();
        }

        public T Save<T>( T entity ) where T : class
        {
            try
            {
                this.data.Set<T>().AddOrUpdate( entity );
                this.data.SaveChanges();
            }
            catch ( Exception ex )
            {
                int a = 0;
            }

            return entity;
            //var idProperty = entity.GetType().GetProperty( "Id" );

            //if ( idProperty != null )
            //{
            //    if ( idProperty.GetValue( entity ) == null )
            //    {
            //        idProperty.SetValue( entity, Guid.NewGuid().ToString() );
            //    }
            //}

            //var id = ( idProperty == null ? Guid.NewGuid().ToString() : idProperty.GetValue( entity ) );

            //this.data[ id.ToString() ] = entity;

            //return entity;
            //return default( T );
        }
    }
}