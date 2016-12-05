using MVCBase.Enums;

namespace MVCCore.Services
{
    public interface IBaseService
    {
        int UserID { get; set; }
        int LocationID { get; }

        int NmvnModuleID { get; }
        GlobalEnums.NmvnTaskID NmvnTaskID { get; }

        GlobalEnums.AccessLevel GetAccessLevel();
        GlobalEnums.AccessLevel GetAccessLevel(int? organizationalUnitID);

        bool GetApprovalPermitted();
        bool GetApprovalPermitted(int? organizationalUnitID);

        bool GetUnApprovalPermitted();
        bool GetUnApprovalPermitted(int? organizationalUnitID);

    }
}
