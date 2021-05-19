﻿using System;

namespace Org.Common.Domain
{
    public class Leave
    {
        public int Id { get; set; }
        
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set;}
        public LeaveStatus Status { get; set; }
        
        public string ApprovedById { get; set; }
        public EmployeePortalUser ApprovedBy { get; set; }
        
        public string LeaveUserId { get; set; }
        public EmployeePortalUser LeaveUser { get; set; }
        
        public string Message { get; set; }
    }
}