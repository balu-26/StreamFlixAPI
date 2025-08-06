public class AccessLog
{
    public int Id { get; set; }
    public string Ip { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Isp { get; set; }
    public string Timezone { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
