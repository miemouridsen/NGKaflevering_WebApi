using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Hubs
{
    public interface INotification
    {
        Task ReceiveNotification(Measurement measurement);
    }
    public class NotificationHub : Hub<INotification>
    {
        public async Task SendMessage(Measurement measurement)
        {
            await Clients.All.ReceiveNotification(measurement);
        }
    }
}
