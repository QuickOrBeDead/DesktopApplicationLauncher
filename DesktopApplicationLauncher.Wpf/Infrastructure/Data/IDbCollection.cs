namespace DesktopApplicationLauncher.Wpf.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IDbCollection<TEntity> where TEntity : class
    {
        IList<TEntity> List(
            Expression<Func<TEntity, bool>> filter = null, 
            Expression<Func<TEntity, object>> orderBy = null, 
            bool ascending = true);

        IList<TDto> List<TDto>(
            Expression<Func<TEntity, TDto>> selectExpression,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool ascending = true);

        TEntity GetById(object id);

        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Update(Expression<Func<TEntity, TEntity>> extend, Expression<Func<TEntity, bool>> predicate);

        void Delete(object id);

        bool ExistsById(object id);
    }
}
