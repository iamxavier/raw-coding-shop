namespace RawCoding.Shop.UI.Workers.Email
{
    public class EmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool Html { get; set; }
    }
}