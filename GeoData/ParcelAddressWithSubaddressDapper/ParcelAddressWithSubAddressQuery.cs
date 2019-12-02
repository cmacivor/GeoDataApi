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
        public async Task<List<ParcelAddressWithSubAddressModel>> Get(string searchString)
        {
            string connString = ConfigurationManager.ConnectionStrings["GISConnectionString"].ToString();

            string sql = @"select top 10 
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
                            from [gp].[GEODATAAPIADDRESSES]";


            //string sql = @"select top 10 
            //                [AddressId],
            //                [AddressLabel],
            //                [BuildingNumber],
            //                [StreetDirection],
            //                [StreetName],
            //                [StreetType],
            //                [ExtensionWithUnit],
            //                [UnitType],
            //                [UnitValue],
            //                [ZipCode],
            //                [Mailable],
            //                [StatePlaneX],
            //                [StatePlaneY],
            //                [CouncilDistrict]
            //                from [gp].[GEODATAAPIADDRESSES]
            //                where AddressLabel like @searchValue + '%'";
                  

            using (var connection = new SqlConnection(connString))
            {
                var rows = await connection.QueryAsync<ParcelAddressWithSubAddressModel>(sql, new { searchValue = searchString });

                return rows.ToList();
            }
        }
    }
}
