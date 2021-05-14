namespace DesktopApplicationLauncher.Wpf.Infrastructure.Business
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

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

        public IList<ApplicationListItemModel> ListAllApplications(int? parentId = null)
        {
            return _dbContext.Applications.ListDto(
                x => new ApplicationListItemModel
                         {
                             Id = x.Id,
                             Name = x.Name,
                             Description = x.Description,
                             Path = x.Path,
                             Arguments = x.Arguments,
                             LastAccessedDate = x.LastAccessedDate,
                             SortOrder = x.SortOrder,
                             ItemType = x.ItemType ?? ApplicationItemType.File,
                             CreateDate = x.CreateDate
                        },
                x => x.ParentId == parentId,
                x => x.SortOrder);
        }

        public int AddApplication(ApplicationAddModel addModel)
        {
            if (addModel == null)
            {
                throw new ArgumentNullException(nameof(addModel));
            }

            var application = AddApplicationInternal(addModel);

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
                             ParentId = updateModel.ParentId,
                             Name = updateModel.Name,
                             Description = updateModel.Description,
                             Path = updateModel.Path,
                             Arguments = updateModel.Arguments
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

        public void ConvertToFolder(int id, string folderName = null)
        {
            var application = _dbContext.Applications.GetById(id);
            if (application.ItemType == ApplicationItemType.Folder)
            {
                return;
            }

            var folder = AddApplicationInternal(new ApplicationAddModel
                                                      {
                                                          Name = folderName ?? application.Name,
                                                          ItemType = ApplicationItemType.Folder,
                                                          ParentId = application.ParentId,
                                                          SortOrder = application.SortOrder
                                                      });
            _dbContext.Applications.Update(
                _ => new Application
                         {
                             ParentId = folder.Id, SortOrder = 0, HierarchyPath = CreateHierarchyPath(folder)
                         },
                x => x.Id == id);
        }

        public IList<ParentFolderModel> GetParentFolders(int? id)
        {
            var result = new List<ParentFolderModel> { new ParentFolderModel { Name = "Home" } };
            if (id == null)
            {
                return result;
            }

            var folder = _dbContext.Applications.ListDto(x => new { x.HierarchyPath, x.Name, x.ParentId }, x => x.Id == id).FirstOrDefault();
            if (folder == null)
            {
                return result;
            }

            if (string.IsNullOrWhiteSpace(folder.HierarchyPath))
            {
                result.Add(new ParentFolderModel { Name = folder.Name });

                return result;
            }

            var parentIds = folder.HierarchyPath.Split('/', StringSplitOptions.RemoveEmptyEntries)
                                                        .Select(x => Convert.ToInt32(x, CultureInfo.InvariantCulture))
                                                        .ToList();
            var parents = _dbContext.Applications
                                                  .ListDto(x => new { x.Id, x.Name }, x => parentIds.Contains(x.Id))
                                                  .ToDictionary(x => x.Id);
            for (var i = 0; i < parentIds.Count; i++)
            {
                var parentId = parentIds[i];
                if (parents.TryGetValue(parentId, out var parent))
                {
                    result.Add(new ParentFolderModel { Id = parent.Id, Name = parent.Name });
                }
            }

            result.Add(new ParentFolderModel { Name = folder.Name });

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1310:Specify StringComparison for correctness", Justification = "Reviewed")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1307:Specify StringComparison for clarity", Justification = "Reviewed")]
        public void MoveToFolder(int sourceId, int targetId)
        {
            var target = _dbContext.Applications.GetById(targetId);
            if (target.ItemType != ApplicationItemType.Folder)
            {
                return;
            }

            var source = _dbContext.Applications.GetById(sourceId);

            var targetHierarchyPath = CreateHierarchyPath(target);
            _dbContext.Applications.Update(
                _ => new Application
                         {
                             ParentId = target.Id,
                             SortOrder = GetMaxSortOrder(target.Id) + 1,
                             HierarchyPath = targetHierarchyPath
                         },
                x => x.Id == sourceId);

            if (source.ItemType == ApplicationItemType.Folder)
            {
                var oldHierarchyPath = CreateHierarchyPath(source);
                var newHierarchyPath = $"{targetHierarchyPath}{sourceId}/";

                _dbContext.Applications.Update(
                    x => new Application
                             {
                                 HierarchyPath = x.HierarchyPath.Replace(oldHierarchyPath, newHierarchyPath)
                             },
                    x => x.HierarchyPath.StartsWith(oldHierarchyPath));
            }
        }

        private Application AddApplicationInternal(ApplicationAddModel addModel)
        {
            var application = new Application
                                  {
                                      ParentId = addModel.ParentId,
                                      Name = addModel.Name,
                                      Description = addModel.Description,
                                      ItemType = addModel.ItemType,
                                      Path = addModel.Path,
                                      Arguments = addModel.Arguments,
                                      SortOrder = addModel.SortOrder,
                                      HierarchyPath = GetHierarchyPath(addModel.ParentId),
                                      CreateDate = DateTime.Now
                                  };
            _dbContext.Applications.Insert(application);
            return application;
        }

        private string GetHierarchyPath(int? parentId)
        {
            string hierarchyPath = null;
            if (parentId.HasValue)
            {
                var parent = _dbContext.Applications.GetById(parentId.Value);
                if (parent != null)
                {
                    hierarchyPath = CreateHierarchyPath(parent);
                }
            }

            return hierarchyPath;
        }

        private static string CreateHierarchyPath(Application parent)
        {
            return string.IsNullOrWhiteSpace(parent.HierarchyPath) ? $"/{parent.Id.ToString(CultureInfo.InvariantCulture)}/" : $"{parent.HierarchyPath}{parent.Id}/";
        }

        public int SaveApplication(ApplicationSaveModel saveModel)
        {
            if (saveModel == null)
            {
                throw new ArgumentNullException(nameof(saveModel));
            }

            if (saveModel.Id > 0)
            {
                UpdateApplication(new ApplicationUpdateModel
                                                          {
                                                              Id = saveModel.Id,
                                                              Name = saveModel.Name,
                                                              Description = saveModel.Description,
                                                              Arguments = saveModel.Arguments,
                                                              Path = saveModel.Path
                                                          });

                return saveModel.Id;
            }

            return AddApplication(new ApplicationAddModel
                                      {
                                          ParentId = saveModel.ParentId,
                                          ItemType = saveModel.ItemType,
                                          Name = saveModel.Name,
                                          Description = saveModel.Description,
                                          Arguments = saveModel.Arguments,
                                          Path = saveModel.Path,
                                          SortOrder = GetMaxSortOrder(saveModel.ParentId) + 1
                                      });
        }

        private int GetMaxSortOrder(int? parentId)
        {
            return _dbContext.Applications.Max(x => x.SortOrder, x => x.ParentId == parentId);
        }
    }
}
