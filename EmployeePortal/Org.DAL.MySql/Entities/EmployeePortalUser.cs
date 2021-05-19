using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Org.DAL.MySql.Entities
{
    internal class EmployeePortalUser :IdentityUser
    {
        public decimal Salary { get; set; }
        public string LeaveInformation { get; set; }
        
        public List<Leave> Leaves { get; set; }
        public List<Leave> ApprovedLeaves { get; set; }
    }
}