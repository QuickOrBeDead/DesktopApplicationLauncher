namespace DesktopApplicationLauncher.Wpf.IntegrationTests.Infrastructure.Business
{
    using System.Collections.Generic;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Business;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Data;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Entities;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Models;

    using LiteDB;

    using NUnit.Framework;

    [TestFixture]
    public sealed class ApplicationServiceFixture
    {
        private IDbContext _liteDbContext;

        private ApplicationService _applicationService;

        [SetUp]
        public void Before()
        {
            var liteDatabase = new LiteDatabase("Filename=IntegrationTests.db");
            foreach (var collectionName in liteDatabase.GetCollectionNames())
            {
                liteDatabase.DropCollection(collectionName);
            }

            _liteDbContext = new LiteDbContext(liteDatabase);
            _applicationService = new ApplicationService(_liteDbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _liteDbContext.Dispose();
        }

        [Test]
        [TestCase(ApplicationItemType.File)]
        [TestCase(ApplicationItemType.Website)]
        [TestCase(ApplicationItemType.Folder)]
        public void SaveApplication_ShouldAddApplication_WhenIdIs0(ApplicationItemType itemType)
        {
            // Arrange
            var parentId = _applicationService.SaveApplication(
                new ApplicationSaveModel
                    {
                        Arguments = "Arguments",
                        Description = "Description",
                        Name = "Name",
                        Path = "Path",
                        ItemType = ApplicationItemType.Folder
                    });

            var applicationIds = new List<int>();

            // Act
            for (var i = 0; i < 3; i++)
            {
                applicationIds.Add(_applicationService.SaveApplication(
                    new ApplicationSaveModel
                        {
                            Arguments = $"Arguments{i}",
                            Description = $"Description{i}",
                            Name = $"Name{i}",
                            Path = $"Path{i}",
                            ItemType = itemType,
                            ParentId = parentId
                        }));
            }

            // Assert
            for (var i = 0; i < applicationIds.Count; i++)
            {
                var id = applicationIds[i];
                var application = _liteDbContext.Applications.GetById(id);

                Assert.AreEqual($"Arguments{i}", application.Arguments);
                Assert.AreEqual($"Description{i}", application.Description);
                Assert.AreEqual($"Name{i}", application.Name);
                Assert.AreEqual($"Path{i}", application.Path);
                Assert.AreEqual(itemType, application.ItemType);
                Assert.AreEqual(parentId, application.ParentId);
                Assert.AreEqual(i + 1, application.SortOrder);
            }
        }
    }
}
