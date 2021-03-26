﻿namespace DesktopApplicationLauncher.Wpf.Infrastructure.Business
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
                             SortOrder = x.SortOrder,
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

            var application = new Application
                                  {
                                      Name = addModel.Name,
                                      Path = addModel.Path,
                                      Arguments = addModel.Arguments,
                                      SortOrder = addModel.SortOrder,
                                      CreateDate = DateTime.Now
                                  };
            _dbContext.Applications.Insert(application);

            return application.Id;
        }

        public void UpdateApplication(ApplicationUpdateModel updateModel)
        {
            if (updateModel == null)
            {
                throw new ArgumentNullException(nameof(updateModel));
            }

            _dbContext.Applications.Update(
                _ => new Application
                         {
                             Name = updateModel.Name, Path = updateModel.Path, Arguments = updateModel.Arguments
                         },
                x => x.Id == updateModel.Id);
        }

        public void UpdateApplicationOrder(int id, int sortOrder)
        {
            _dbContext.Applications.Update(_ => new Application { SortOrder = sortOrder }, x => x.Id == id);
        }

        public void DeleteApp(int id)
        {
            _dbContext.Applications.Delete(id);
        }

        public DateTime UpdateApplicationLastAccessDate(int id)
        {
            var lastAccessedDate = DateTime.Now;

            _dbContext.Applications.Update(_ => new Application { LastAccessedDate = lastAccessedDate }, x => x.Id == id);

            return lastAccessedDate;
        }
    }
}
