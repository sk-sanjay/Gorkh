using System.Collections.Generic;
namespace Application.ViewModels
{
    public class NotificationsVM : BaseVM
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public List<NotificationDetailsVM> NotificationDetails { get; set; }
    }
}
