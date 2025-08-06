namespace StreamFlixAPI.DTOs
{
    public class GeoInfo
    {
        public string Query { get; set; }       // IP
        public string Country { get; set; }
        public string RegionName { get; set; }
        public string City { get; set; }
        public string Isp { get; set; }
        public string Org { get; set; }
        public string Timezone { get; set; }
    }
}
