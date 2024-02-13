namespace BackgroundTaskHelper.Models
{
    public class LogModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        public string AccessedURL { get; set; }= string.Empty;

        public string IpAddress { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public string Response { get; set; }=string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Timestamp { get; set; } = string.Empty;
    }
}
