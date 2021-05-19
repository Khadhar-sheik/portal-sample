using System.Collections.Generic;

namespace Org.DAL.MySql.Entities
{
    internal class Employee
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public string LeaveInformation { get; set; }
    }
}