using QLBH_Dion.Models;
using System.Threading.Tasks;

namespace QLBH_Dion.Repository.Interfaces
{
    public interface INotificationHub
    {
        Task ReceiveNotification(Notification obj);
    }
}
