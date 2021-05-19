using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Org.Common.DataProvider;
using Org.Common.Domain;
using Leave = Org.Common.Model.Leave;

namespace Org.DAL.MySql.DataProvider
{
    internal class LeaveDataProvider : ILeaveDataProvider
    {
        private readonly EmployeContext _context;

        public LeaveDataProvider(EmployeContext context)
        {
            _context = context;
        }

        public async Task<Leave> Create(Leave leave)
        {
            var entity = new Common.Domain.Leave
            {
                Id = leave.Id,
                From = leave.From,
                To = leave.To,
                Status = LeaveStatus.Submitted,
                LeaveUserId = leave.LeaveUserId
            };

            var result = await _context.Leaves.AddAsync(entity);
            leave.Id = result.Entity.Id;
            return leave;
        }

        public Task<Leave> Get(int leaveId)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateLeaveStatus(int leaveId, LeaveStatus status)
        {
            var entry = await _context.Leaves.FirstOrDefaultAsync(l => l.Id == leaveId);
            entry.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}