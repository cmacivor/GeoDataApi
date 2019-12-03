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

            string sql = string.Empty;

            string directionSearchTerm = string.Empty;

            sql = @"";

            directionSearchTerm = HandleDirections(searchString, directionSearchTerm);

            if (!string.IsNullOrEmpty(directionSearchTerm))
            {
                sql = @"SELECT TOP (10) [UniqueID]
                          ,[AddressID]
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
                      where AddressLabel like @directionSearchValue
                      UNION ALL
                      SELECT TOP (10) [UniqueID]
                          ,[AddressID]
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
                      where AddressLabel like @searchValue";

                using (var connection = new SqlConnection(connString))
                {
                    var rows = await connection.QueryAsync<GeoDataApiViewQueryResult>(sql, new { searchValue = searchString + "%", directionSearchValue = directionSearchTerm + "%" });

                    return rows.ToList();
                }
            }
            else
            {
                sql = @" SELECT TOP (10) [UniqueID]
                          ,[AddressID]
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
                      where AddressLabel like @searchValue";


                using (var connection = new SqlConnection(connString))
                {
                    var rows = await connection.QueryAsync<GeoDataApiViewQueryResult>(sql, new { searchValue = searchString + "%" });

                    return rows.ToList();
                }
            }
        }

        private string HandleDirections(string searchString, string directionSearchTerm)
        {
            if (searchString.IndexOf("West", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                directionSearchTerm = Regex.Replace(searchString, "West", "w", RegexOptions.IgnoreCase);
            }
            else if (searchString.IndexOf("East", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                directionSearchTerm = Regex.Replace(searchString, "East", "e", RegexOptions.IgnoreCase);
            }
            else if (searchString.IndexOf("South", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                directionSearchTerm = Regex.Replace(searchString, "South", "s", RegexOptions.IgnoreCase);
            }
            else if (searchString.IndexOf("North", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                directionSearchTerm = Regex.Replace(searchString, "North", "w", RegexOptions.IgnoreCase);
            }

            return directionSearchTerm;
        }
    }
}
