using Microsoft.AspNetCore.Identity;

namespace Org.Common.Domain
{
    public class EmployeePortalUser : IdentityUser
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Salary { get; set; }
        public string LeaveInformation { get; set; }
    }
}