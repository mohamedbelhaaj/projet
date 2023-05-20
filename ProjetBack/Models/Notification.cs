namespace ProjetBack.Models
{
    public class Notification
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ActionLink { get; set; }

        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}