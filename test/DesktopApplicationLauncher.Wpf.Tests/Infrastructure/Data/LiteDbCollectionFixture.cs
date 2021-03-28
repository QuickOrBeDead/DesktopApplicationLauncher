namespace DesktopApplicationLauncher.Wpf.Tests.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Data;
    using DesktopApplicationLauncher.Wpf.Tests.Infrastructure.Entities;

    using LiteDB;

    using NSubstitute;

    using NUnit.Framework;

    [TestFixture]
    public sealed class LiteDbCollectionFixture
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void List(bool ascending)
        {
            // Arrange
            var expected = new List<TestEntity>();
            Expression<Func<TestEntity, bool>> filter = x => x.Name == "Test";
            Expression<Func<TestEntity, object>> orderBy = x => x.Id;

            var liteDbCollection = CreateLiteDbCollection(out var query);
            query.ToList().Returns(expected);

            // Act
            var actual = liteDbCollection.List(filter, orderBy, ascending);

            // Assert
            Assert.AreSame(expected, actual);

            AssertFilterAndOrderByQuery(query, filter, orderBy, ascending);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void List_WhenFilterIsNull(bool ascending)
        {
            // Arrange
            var expected = new List<TestEntity>();
            Expression<Func<TestEntity, object>> orderBy = x => x.Id;

            var liteDbCollection = CreateLiteDbCollection(out var query);
            query.ToList().Returns(expected);

            // Act
            var actual = liteDbCollection.List(orderBy: orderBy, ascending: ascending);

            // Assert
            Assert.AreSame(expected, actual);

            AssertFilterAndOrderByQuery(query, orderBy: orderBy, ascending: ascending);
        }

        [Test]
        public void List_WhenOrderByIsNull()
        {
            // Arrange
            var expected = new List<TestEntity>();
            Expression<Func<TestEntity, bool>> filter = x => x.Name == "Test";

            var liteDbCollection = CreateLiteDbCollection(out var query);
            query.ToList().Returns(expected);

            // Act
            var actual = liteDbCollection.List(filter);

            // Assert
            Assert.AreSame(expected, actual);

            AssertFilterAndOrderByQuery(query, filter);
        }

        [Test]
        public void List_WhenFilterAndOrderByAreNull()
        {
            // Arrange
            var expected = new List<TestEntity>();

            var liteDbCollection = CreateLiteDbCollection(out var query);
            query.ToList().Returns(expected);

            // Act
            var actual = liteDbCollection.List();

            // Assert
            Assert.AreSame(expected, actual);

            AssertFilterAndOrderByQuery(query);
        }

        private static void AssertFilterAndOrderByQuery(
            ILiteQueryable<TestEntity> query,
            Expression<Func<TestEntity, bool>> filter = null,
            Expression<Func<TestEntity, object>> orderBy = null,
            bool ascending = true)
        {
            if (filter == null)
            {
                query.DidNotReceive().Where(Arg.Any<Expression<Func<TestEntity, bool>>>());
            }
            else
            {
                query.Received(1).Where(filter);
            }

            if (orderBy == null)
            {
                query.DidNotReceive().OrderBy(Arg.Any<Expression<Func<TestEntity, object>>>());
                query.DidNotReceive().OrderByDescending(Arg.Any<Expression<Func<TestEntity, object>>>());
            }
            else
            {
                if (ascending)
                {
                    query.Received(1).OrderBy(orderBy);
                }
                else
                {
                    query.Received(1).OrderByDescending(orderBy);
                }
            }
        }

        private static LiteDbCollection<TestEntity> CreateLiteDbCollection(out ILiteQueryable<TestEntity> query)
        {
            var liteDatabase = Substitute.For<ILiteDatabase>();
            query = CreateLiteQueryable<TestEntity>(liteDatabase);
            var liteDbCollection = CreateLiteDbCollection<TestEntity>(liteDatabase);
            return liteDbCollection;
        }

        private static ILiteQueryable<TEntity> CreateLiteQueryable<TEntity>(ILiteDatabase liteDatabase)
        {
            var liteQueryable = Substitute.For<ILiteQueryable<TEntity>>();
            var liteCollection = Substitute.For<ILiteCollection<TEntity>>();
            liteCollection.Query().Returns(liteQueryable);
            liteDatabase.GetCollection<TEntity>(typeof(TEntity).Name).Returns(liteCollection);

            return liteQueryable;
        }

        private static LiteDbCollection<TEntity> CreateLiteDbCollection<TEntity>(ILiteDatabase liteDatabase)
            where TEntity : class
        {
            return new LiteDbCollection<TEntity>(liteDatabase);
        }
    }
}
