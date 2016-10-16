
using Data;
using Domain;
using Moq;
using Moq.Protected;
using Web.Api;
using WebApplication.Controllers;

namespace Tests.Common
{
    public class ControllerMock
    {
        public static ISystemUser DefaultSystemUser
        {
            get
            {
                var user = new Mock<ISystemUser>();
                user.Setup(x => x.CompanyId)
                    .Returns(1169);

                return user.Object;
            }
        }
        public static Mock<T> CreateMvcController<T>(IDataContext dbContext, ISystemUser systemUser) where T : BaseController
        {
            var ctrl = new Mock<T>
            {
                CallBase = true,
            };
            ctrl.Protected()
                .SetupGet<IDataContext>("DataContext")
                .Returns(dbContext);
            ctrl.Protected()
                .SetupGet<ISystemUser>("SystemUser")
                .Returns(systemUser);

            return ctrl;
        }

        public static Mock<T> CreateApiController<T>(IDataContext dbContext,  ISystemUser systemUser) where T : ChqApiController
        {
            var ctrl = new Mock<T>
            {
                CallBase = true,
            };
            ctrl.Protected()
                .SetupGet<IDataContext>("DataContext")
                .Returns(dbContext);
            ctrl.Protected()
                .SetupGet<ISystemUser>("SystemUser")
                .Returns(systemUser);

            return ctrl;
        }
    }
}
