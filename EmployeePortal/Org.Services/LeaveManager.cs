using System;
using System.Threading.Tasks;
using Org.Common.DataProvider;
using Org.Common.Domain;
using Org.Common.Manager;
using Leave = Org.Common.Model.Leave;

namespace Org.Services
{
    internal class LeaveManager : ILeaveManager
    {
        private readonly ILeaveDataProvider _leaveDataProvider;

        public LeaveManager(ILeaveDataProvider leaveDataProvider)
        {
            _leaveDataProvider = leaveDataProvider;
        }

        public async Task<Leave> Create(Leave leave)
        {
            return await _leaveDataProvider.Create(leave);
        }

        public async Task<Leave> Get(int leaveId)
        {
            return await _leaveDataProvider.Get(leaveId);
        }

        public async Task Approve(int leaveId)
        {
            var leave = await Get(leaveId);
            if (leave.Status == LeaveStatus.Rejected)
            {
                throw new InvalidOperationException("Leave has been already rejected");
            }
            
            await _leaveDataProvider.UpdateLeaveStatus(leaveId, LeaveStatus.Approved);
        }
    }
}