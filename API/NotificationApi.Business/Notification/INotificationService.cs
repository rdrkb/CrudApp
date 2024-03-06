using Contracts;
using NotificationApi.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationApi.Business.Notification
{
    public interface INotificationService
    {
        Task CreateNotification(UserNotification userNotification);
        Task<List<UserNotification>> GetNotifications(int pageNumber, int pageSize);
        Task<long> GetNumberOfNotification();
    }
}
