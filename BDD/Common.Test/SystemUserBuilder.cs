
using Domain;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace ClinicHQ.Tests.Common
{
    public class SystemUserBuilder
    {
        private string UserName = "asd";
        private int Id = 4;
        private int RoleId = 4;
        private int CompanyId = 1169;
        private List<int> Clinics = new List<int> { 1169 };
        private string FullName = "asd dsa";
        private string Email = "asd@mailinator.org";
        private bool CanPrescribeDrugs = false;
        private HashSet<string> Permissions = new HashSet<string>();

        public SystemUserBuilder()
        {
        }

        public SystemUserBuilder WithClinicId(int clinicId)
        {
            CompanyId = clinicId;
            return this;
        }

        public SystemUserBuilder WithUserId(int userID)
        {
            Id = userID;
            return this;
        }
        public SystemUserBuilder WithUserName(string userName)
        {
            UserName = userName;
            return this;
        }
        public SystemUserBuilder WithFullName(string fullName)
        {
            FullName = fullName;
            return this;
        }
        public SystemUserBuilder WithRoleId(int roleId)
        {
            RoleId = roleId;
            return this;
        }
        public SystemUserBuilder WithClinics(List<int> clinicIds)
        {
            Clinics = clinicIds.ToList();
            return this;
        }

        public SystemUserBuilder WithPermissions(List<string> permissions)
        {
            Permissions.Clear();
            permissions.ForEach(x => Permissions.Add(x));
            return this;
        }

        public SystemUserBuilder WithEmail(string email)
        {
            Email = email;
            return this;
        }

        public SystemUserBuilder WithDrugsPerscriptionPrivilege(bool canDrugAnimals)
        {
            CanPrescribeDrugs = canDrugAnimals;
            return this;
        }
        

        public ISystemUser Build()
        {
            var systemUser = new Mock<ISystemUser>();
            systemUser.SetupGet(x => x.UserId)
                      .Returns(Id);
            systemUser.SetupGet(x => x.RoleId)
                      .Returns(RoleId);
            systemUser.SetupGet(x => x.CompanyId)
                      .Returns(CompanyId);
            systemUser.SetupGet(x => x.FullName)
                      .Returns(FullName);
            systemUser.SetupGet(x => x.Email)
                      .Returns(Email);
            systemUser.SetupGet(x => x.Permissions)
                      .Returns(Permissions.ToList());
            systemUser.SetupGet(x => x.UserName)
                      .Returns(UserName);
            systemUser.SetupGet(x => x.CanPrescribeDrugs)
                      .Returns(CanPrescribeDrugs);
            systemUser.Setup(x => x.HasPermissions(It.IsAny<string>()))
                      .Callback((string s) => Permissions.Contains(s));
            return systemUser.Object;
        }
    }
}
