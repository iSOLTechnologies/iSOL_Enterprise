using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iSOL_Enterprise.Models
{
    public class UsersModels
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ContactNumber { get; set; }
        public string? Username { get; set; }
        public string? UserCode { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsSession { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? UserPic { get; set; }
        public string? RoleCode { get; set; }
        public string? RoleName { get; set; }
        public int SuperiorId { get; set; }
        public int Department { get; set; }
        public int Branch { get; set; }
        public string? RegionCode { get; set; }
        public string? DepartmentCode { get; set; }
        public string? DepartmentName { get; set; }
        public string? EmployeeOfName { get; set; }
        public string? EmployeeOf { get; set; }
        public int IsQc { get; set; }
        public string? Guid { get; set; }
        public string? LogType { get; set; }
        public int UserType { get; set; }
        public string? IpAddress { get; set; }
        public string? MachineName { get; set; }
        public string? CurrentPassword { get; set; }
        public bool? IsActive { get; set; }
        public bool? RowStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public List<Attachments> ListAttachments { get; set; }
        public string? WebRootPath { get; set; }



    }
    public class _usersModels : UsersModels
    {
        [ScaffoldColumn(false)]
        [DataObjectFieldAttribute(true, true, false)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public string Contents { get; set; }
        public byte[] Image { get; set; }
        public List<PagesModels> ListPages { get; set; }
        public List<ModulesModels> listModules { get; set; }
        public List<ModulesModels> Modules { get; set; }
        public List<_usersModels> ListItems { get; set; }
        public List<Setup_DepartmentsModels> ListDepartments { get; set; }
        public List<RoleModels> ListRoles { get; set; }
        public List<RegionModels> ListRegions { get; set; }
        public List<_usersModels> ListSuperiors { get; set; }
        public List<_usersModels> ListUsers { get; set; }
       // public List<EmployeeOfModels> ListEmployeeOf { get; set; }
    }
}
