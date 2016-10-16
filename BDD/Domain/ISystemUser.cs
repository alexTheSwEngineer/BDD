using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface ISystemUser
    {
        int UserId { get; }
        int RoleId { get; }
        int CompanyId { get; }
        string FullName { get; }
        string Email { get; }
        List<string> Permissions { get; }
        string UserName { get; }
        bool CanPrescribeDrugs { get; }
        bool HasPermissions(string permission);
    }
}
