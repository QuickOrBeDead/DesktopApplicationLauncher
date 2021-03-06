namespace DesktopApplicationLauncher.Wpf.Infrastructure.Business
{
    using System.Collections.Generic;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Models;

    public interface IApplicationService
    {
        IList<ApplicationListItemModel> ListAllApplications();

        int AddApplication(ApplicationAddModel addModel);

        void UpdateApplication(ApplicationUpdateModel updateModel);

        void UpdateApplicationOrder(int id, int sortOrder);

        void DeleteApp(int id);
    }
}