using Microsoft.AspNetCore.SignalR;
using QLBH_Dion.Models;
using QLBH_Dion.Repository.Interfaces;
using System.Threading.Tasks;
namespace QLBH_Dion.Util
{
    public class NotificationHub : Hub<INotificationHub>
    {
        public async Task ReceiveNotification(Notification obj)
        {
            await Clients.All.ReceiveNotification(obj);
        }
    }
}
