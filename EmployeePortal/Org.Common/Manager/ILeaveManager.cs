using System.Threading.Tasks;
using Org.Common.Model;

namespace Org.Common.Manager
{
    public interface ILeaveManager
    {
        Task<Leave> Create(Leave leave);
        Task Approve(int leaveId);
    }
}