using GeoData.ParcelAddressWithSubaddressDapperModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GeoData.ParcelAddressWithSubaddressDapper
{
    public class ParcelAddressWithSubAddressQuery
    {
        public  List<ParcelAddressWithSubAddressModel> Get(string searchString)
        {
            string connString = ConfigurationManager.ConnectionStrings["GISConnectionString"].ToString();

            string sql = @"SELECT  
                           [OBJECTID]
                          ,[AddressId]
                          ,[AddressLabel]
                          ,[BuildingNumber]
                          ,[StreetDirection]
                          ,[StreetName]
                          ,[StreetType]
                          ,[ExtensionWithUnit]
                          ,[UnitType]
                          ,[UnitValue]
                          ,[ZipCode]
                          ,[Mailable]
                          ,[StatePlaneX]
                          ,[StatePlaneY]
                          ,[Latitude]
                          ,[Longitude]
                          ,[CreatedBy]
                          ,[CreatedDate]
                          ,[EditBy]
                          ,[EditDate]
                          ,[Shape]
                      FROM [GeodataSummary].[gp].[addr_ParcelAddressWithSubaddress]
                      where AddressLabel like '@searchString' + '%'";

            using (var connection = new SqlConnection(connString))
            {
                var rows = connection.Query<ParcelAddressWithSubAddressModel>(sql, new { @searchString = searchString }).ToList();

                return rows;
            }
        }
    }
}
