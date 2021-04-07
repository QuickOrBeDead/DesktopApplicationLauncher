namespace DesktopApplicationLauncher.Wpf.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    [SuppressMessage("Naming", "CA1711: Identifiers should not have incorrect suffix", Justification = "Reviewed")]
    public interface IDbCollection<TEntity> where TEntity : class
    {
        IList<TEntity> List(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool ascending = true);

        IList<TDto> ListDto<TDto>(
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

        IList<TEntity> GetByIds(object[] ids);

        TKey Max<TKey>(Expression<Func<TEntity, TKey>> selectExpression, Expression<Func<TEntity, bool>> filter = null)
            where TKey : struct;

        TKey Min<TKey>(Expression<Func<TEntity, TKey>> selectExpression, Expression<Func<TEntity, bool>> filter = null)
            where TKey : struct;
    }
}
