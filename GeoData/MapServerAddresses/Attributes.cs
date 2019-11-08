namespace GeoData.MapServer
{
    public class Attributes
    {
        public int OBJECTID { get; set; }
        public string AddressId { get; set; }
        public string AddressLabel { get; set; }
        public string BuildingNumber { get; set; }
        public string StreetDirection { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string ExtensionWithUnit { get; set; }
        public string UnitType { get; set; }
        public string UnitValue { get; set; }
        public string ZipCode { get; set; }
        public string Mailable { get; set; }
        public double StatePlaneX { get; set; }
        public double StatePlaneY { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public object CreatedBy { get; set; }
        public object CreatedDate { get; set; }
        public string EditBy { get; set; }
        public long EditDate { get; set; }
    }
}
