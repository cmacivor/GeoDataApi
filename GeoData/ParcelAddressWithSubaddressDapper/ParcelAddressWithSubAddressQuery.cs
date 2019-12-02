using GeoData.ParcelAddressWithSubaddressDapperModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Text.RegularExpressions;

namespace GeoData.ParcelAddressWithSubaddressDapper
{
    public class ParcelAddressWithSubAddressQuery
    {
        public async Task<List<ParcelAddressWithSubAddressModel>> Get(string searchString)
        {
            string connString = ConfigurationManager.ConnectionStrings["GISConnectionString"].ToString();

            string sql = string.Empty;

            string directionSearchTerm = string.Empty;

            sql = @"";

            directionSearchTerm = HandleDirections(searchString, directionSearchTerm);

            if (!string.IsNullOrEmpty(directionSearchTerm))
            {
                sql = @"select top 10 
                        [AddressId],
                        [AddressLabel],
                        [BuildingNumber],
                        [StreetDirection],
                        [StreetName],
                        [StreetType],
                        [ExtensionWithUnit],
                        [UnitType],
                        [UnitValue],
                        [ZipCode],
                        [Mailable],
                        [StatePlaneX],
                        [StatePlaneY],
                        [CouncilDistrict]
                        from [gp].[addr_GeodataAPIAddresses]
                        where AddressLabel like @directionSearchValue
                        UNION ALL
                        select top 10 
                        [AddressId],
                        [AddressLabel],
                        [BuildingNumber],
                        [StreetDirection],
                        [StreetName],
                        [StreetType],
                        [ExtensionWithUnit],
                        [UnitType],
                        [UnitValue],
                        [ZipCode],
                        [Mailable],
                        [StatePlaneX],
                        [StatePlaneY],
                        [CouncilDistrict]
                        from [gp].[addr_GeodataAPIAddresses]
                        where AddressLabel LIKE @searchValue";

                using (var connection = new SqlConnection(connString))
                {
                    var rows = await connection.QueryAsync<ParcelAddressWithSubAddressModel>(sql, new { searchValue = searchString + "%", directionSearchValue = directionSearchTerm + "%" });

                    return rows.ToList();
                }
            }
            else
            {
                sql = @"select 
                        [AddressId],
                        [AddressLabel],
                        [BuildingNumber],
                        [StreetDirection],
                        [StreetName],
                        [StreetType],
                        [ExtensionWithUnit],
                        [UnitType],
                        [UnitValue],
                        [ZipCode],
                        [Mailable],
                        [StatePlaneX],
                        [StatePlaneY],
                        [CouncilDistrict]
                        from [gp].[addr_GeodataAPIAddresses]
                        where AddressLabel LIKE @searchValue;";

                using (var connection = new SqlConnection(connString))
                {
                    var rows = await connection.QueryAsync<ParcelAddressWithSubAddressModel>(sql, new { searchValue = searchString + "%" });

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
            else if (searchString.IndexOf("South", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                directionSearchTerm = Regex.Replace(searchString, "West", "w", RegexOptions.IgnoreCase);
            }

            return directionSearchTerm;
        }
    }
}
