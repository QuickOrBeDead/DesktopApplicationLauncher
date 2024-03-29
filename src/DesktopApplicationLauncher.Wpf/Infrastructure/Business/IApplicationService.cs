﻿namespace DesktopApplicationLauncher.Wpf.Infrastructure.Business
{
    using System;
    using System.Collections.Generic;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Models;

    public interface IApplicationService
    {
        IList<ApplicationListItemModel> ListAllApplications(int? parentId = null);

        int AddApplication(ApplicationAddModel addModel);

        void UpdateApplication(ApplicationUpdateModel updateModel);

        int SaveApplication(ApplicationSaveModel saveModel);

        void UpdateApplicationOrder(int id, int sortOrder);

        void DeleteApp(int id);

        DateTime UpdateApplicationLastAccessDate(int id);

        void ConvertToFolder(int id, string folderName = null);

        IList<ParentFolderModel> GetParentFolders(int? id);

        void MoveToFolder(int sourceId, int targetId);
    }
}