namespace DesktopApplicationLauncher.Wpf.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    using LiteDB;

    [SuppressMessage("Naming", "CA1711: Identifiers should not have incorrect suffix", Justification = "Reviewed")]
    public sealed class LiteDbCollection<TEntity> : IDbCollection<TEntity>
        where TEntity : class
    {
        private readonly ILiteCollection<TEntity> _collection;

        public LiteDbCollection(ILiteDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            _collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public IList<TEntity> List(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> orderBy = null, bool ascending = true)
        {
            var query = Query(filter, orderBy, ascending);

            return query.ToList();
        }

        public IList<TDto> ListDto<TDto>(
            Expression<Func<TEntity, TDto>> selectExpression,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> orderBy = null,
            bool ascending = true)
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            var query = Query(filter, orderBy, ascending);

            return query.Select(selectExpression).ToList();
        }

        public TEntity GetById(object id)
        {
            return _collection.FindOne($"$._id = {id}");
        }

        public bool ExistsById(object id)
        {
            return _collection.Exists($"$._id = {id}", Array.Empty<BsonValue>());
        }

        public void Insert(TEntity entity)
        {
            _collection.Insert(entity);
        }

        public void Update(TEntity entity)
        {
            _collection.Update(entity);
        }

        public void Update(Expression<Func<TEntity, TEntity>> extend, Expression<Func<TEntity, bool>> predicate)
        {
            _collection.UpdateMany(extend, predicate);
        }

        public void Delete(object id)
        {
            _collection.Delete(new BsonValue(id));
        }

        private ILiteQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> orderBy, bool ascending)
        {
            var query = _collection.Query();

            if (filter != null)
            {
                query.Where(filter);
            }

            if (orderBy != null)
            {
                if (ascending)
                {
                    query.OrderBy(orderBy);
                }
                else
                {
                    query.OrderByDescending(orderBy);
                }
            }

            return query;
        }
    }
}