namespace DesktopApplicationLauncher.Wpf.Infrastructure.Business
{
    using System.Collections.Generic;

    using DesktopApplicationLauncher.Wpf.Infrastructure.Models;

    public interface IApplicationService
    {
        IList<ApplicationListItemModel> ListAllApplications();

        int AddApplication(ApplicationAddModel addModel);
    }
}