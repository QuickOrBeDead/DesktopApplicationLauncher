namespace DesktopApplicationLauncher.Wpf.Infrastructure.Business
{
    using System;
    using System.Collections.Generic;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Data;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Entities;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Models;

    public sealed class ApplicationService : IApplicationService
    {
        private readonly IDbContext _dbContext;

        public ApplicationService(IDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IList<ApplicationListItemModel> ListAllApplications()
        {
            return _dbContext.Applications.ListDto(
                x => new ApplicationListItemModel
                         {
                             Id = x.Id,
                             Name = x.Name,
                             Path = x.Path,
                             Arguments = x.Arguments,
                             LastAccessedDate = x.LastAccessedDate,
                             CreateDate = x.CreateDate
                         }, 
                orderBy: x => x.SortOrder);
        }

        public int AddApplication(ApplicationAddModel addModel)
        {
            if (addModel == null)
            {
                throw new ArgumentNullException(nameof(addModel));
            }

            var applicationId = InsertApplication(addModel);
            UpdateSortOrder(applicationId, applicationId);
            return applicationId;
        }

        private void UpdateSortOrder(int applicationId, int sortOrder)
        {
            _dbContext.Applications.Update(x => new Application { SortOrder = sortOrder }, x => x.Id == applicationId);
        }

        private int InsertApplication(ApplicationAddModel addModel)
        {
            var application = new Application
                                  {
                                      Name = addModel.Name,
                                      Path = addModel.Path,
                                      Arguments = addModel.Arguments,
                                      CreateDate = DateTime.Now
                                  };
            _dbContext.Applications.Insert(application);

            return application.Id;
        }
    }
}
