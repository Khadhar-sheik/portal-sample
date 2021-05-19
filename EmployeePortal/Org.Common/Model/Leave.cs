using System;
using Org.Common.Domain;

namespace Org.Common.Model
{
    public class Leave
    {
        public int Id { get; set; }
        
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set;}
        public LeaveStatus Status { get; set; }
        public string LeaveUserId { get; set; }
    }
}