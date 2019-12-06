namespace GeoData.GeoDataApiViewDapperQuery
{
    public  interface IMapServerReturnResult
    {
         string AddressId { get; set; }
         string BuildingNumber { get; set; }
         string CongressionalDistrict { get; set; }
         string CouncilDistrict { get; set; }
         string HouseDistrict { get; set; }
         string LandUse { get; set; }
         string Mailable { get; set; }
         string OwnerName { get; set; }
         string ParcelID { get; set; }
         string PIN { get; set; }
         string SenateDistrict { get; set; }
         string StreetDirection { get; set; }
         string StreetName { get; set; }
         string UnitType { get; set; }
         string UnitValue { get; set; }
         string VoterPrecinctNumber { get; set; }
         string ZipCode { get; set; }
    }
}