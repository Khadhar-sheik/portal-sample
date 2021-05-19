using System.Threading.Tasks;
using Org.Common.Domain;
using Leave = Org.Common.Model.Leave;

namespace Org.Common.DataProvider
{
    public interface ILeaveDataProvider
    {
        Task<Leave> Create(Leave leave);
        Task<Leave> Get(int leaveId);
        Task UpdateLeaveStatus(int leaveId, LeaveStatus status);
    }
}