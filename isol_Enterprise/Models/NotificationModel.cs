namespace iSOL_Enterprise.Models
{
    public class NotificationModel
    {
        public int MaxId { get;set; }

        public int TotalNotifications { get; set; }
        public bool isNew { get; set; } = false;
    }
}
