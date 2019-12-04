using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeoData.GeoDataApiViewDapperQuery
{
    public class GeoDataApiViewQuery
    {
        public async Task<List<GeoDataApiViewQueryResult>> Get(string searchString)
        {
            string connString = ConfigurationManager.ConnectionStrings["GISConnectionString"].ToString();
                               
            string sql = @"SELECT 
                        [AddressID]
                        ,[AddressLabel]
                        ,[BuildingNumber]
                        ,[StreetDirection]
                        ,[StreetName]
                        ,[StreetType]
                        ,[Extension]
                        ,[UnitType]
                        ,[UnitValue]
                        ,[ZipCode]
                        ,[Mailable]
                        ,[PIN]
                        ,[ParcelID]
                        ,[LandUse]
                        ,[OwnerName]
                        ,[VoterPrecinctNumber]
                        ,[CouncilDistrict]
                        ,[HouseDistrict]
                        ,[SenateDistrict]
                        ,[CongressionalDistrict]
                        ,[StatePlaneX]
                        ,[StatePlaneY]
                    FROM [GeodataSummary].[gp].[addr_GeodataAPIView]
                    where AddressLabel = @searchValue";

                using (var connection = new SqlConnection(connString))
                {
                    var rows = await connection.QueryAsync<GeoDataApiViewQueryResult>(sql, new { searchValue = searchString });

                    return rows.ToList();
                }
         
            }
        }
}
