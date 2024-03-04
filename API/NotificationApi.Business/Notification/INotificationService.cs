using Contracts;
using NotificationApi.Contracts.Models;
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
    }
}
