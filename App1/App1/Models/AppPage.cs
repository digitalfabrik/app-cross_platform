namespace App1.Models
{
    public class DeepLinkPage
    {
        public AppPage Page { get; set; }
        public string Id { get; set; }
    }
    public enum AppPage
    {
        Categories,
        Events,
        Settings,
        Locations,
        Language,
        Extras,
        News,
        Notifications
    }
}
