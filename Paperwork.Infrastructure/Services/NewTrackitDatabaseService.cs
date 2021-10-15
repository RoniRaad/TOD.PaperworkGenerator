using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paperwork.Core.Models;
using System.Data.SqlClient;
using System.Data;
using Paperwork.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace Paperwork.Infrastructure.Services
{
    public class NewTrackitDatabaseService : IDatabaseService
    {
        private readonly TrackitConfig _trackitConfig;
        public NewTrackitDatabaseService(IOptions<TrackitConfig> trackitConfig)
        {
            _trackitConfig = trackitConfig.Value;
        }

        public TrackitEquipment GetTrackitItemInfo(string todNum)
        {
            List<TrackitEquipment> trackitEquiptment = new List<TrackitEquipment>();
            using (SqlConnection con = new SqlConnection(_trackitConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT TOP(1) * FROM [_SMDBA_].[_INVENTORY_] WHERE NAME='{todNum}';", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var serialNum = !reader.IsDBNull("SERIAL_NUMBER") ? reader.GetString("SERIAL_NUMBER") : "";
                            var type = !reader.IsDBNull("INV_CCTXT02") ? reader.GetString("INV_CCTXT02") : "";
                            var clientSequence = !reader.IsDBNull("CLIENT") ? reader.GetInt32("CLIENT").ToString() : "";
                            var deptSequence = !reader.IsDBNull("DEPT") ? reader.GetInt32("DEPT").ToString() : "";
                            var locationSequence = !reader.IsDBNull("LOCATION") ? reader.GetInt32("LOCATION").ToString() : "";
                            var userName = GetUserNameFromSequence(clientSequence);
                            var location = GetLocationFromSequence(locationSequence);
                            var dept = GetAssetDeptFromSequence(deptSequence);

                            trackitEquiptment.Add(new TrackitEquipment()
                            {
                                CurrentUser = userName,
                                ServiceTag = serialNum,
                                Department = dept,
                                Location = location,
                                Type = type,
                                TodNum = todNum,
                                WorkOrderNum = GetWorkOrderNumber(todNum) ?? "N/A"
                            });

                        }
                    }
                }
            }

            return trackitEquiptment.FirstOrDefault<TrackitEquipment>();
        }

        public string GetUserNameFromSequence(string sequence)
        {
            using (SqlConnection con = new SqlConnection(_trackitConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT TOP(1) * FROM [_SMDBA_].[_CUSTOMER_] WHERE SEQUENCE='{sequence}';", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           var firstName = !reader.IsDBNull("FNAME") ? reader.GetString("FNAME") : "";
                           var lastName = !reader.IsDBNull("NAME") ? reader.GetString("NAME") : "";

                            return $"{firstName} {lastName}";
                        }
                    }
                }
            }

            return "";
        }

        public string GetAssetDeptFromSequence(string sequence)
        {
            using (SqlConnection con = new SqlConnection(_trackitConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT TOP(1) * FROM [_SMDBA_].[_DEPART_] WHERE SEQUENCE='{sequence}';", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var departName = !reader.IsDBNull("NAME") ? reader.GetString("NAME") : "";

                            return departName;
                        }
                    }
                }
            }

            return "";
        }

        public string GetLocationFromSequence(string sequence)
        {
            using (SqlConnection con = new SqlConnection(_trackitConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT TOP(1) * FROM [_SMDBA_].[_LOCATION_] WHERE SEQUENCE='{sequence}';", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var departName = !reader.IsDBNull("NAME") ? reader.GetString("NAME") : "";

                            return departName;
                        }
                    }
                }
            }

            return "";
        }

        public string GetWorkOrderNumber(string todNum)
        {
            List<TrackitEquipment> trackitEquiptment = new List<TrackitEquipment>();
            using (SqlConnection con = new SqlConnection(_trackitConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT TOP(1) TI11_WOID FROM [_SMDBA_].[_WORKORD_] WHERE DESCRIPTION LIKE '%{todNum}%' AND [DATE OPEN] > DATEADD(day, -15, GETDATE()) ORDER BY TI11_WOID DESC;", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return !reader.IsDBNull("TI11_WOID") ? reader.GetString("TI11_WOID") : "N/A";
                        }
                    }
                }
            }

            return "N/A";
        }

        public IList<string> GetLocations()
        {
            List<string> locations = new List<string>();
            using (SqlConnection con = new SqlConnection(_trackitConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT [NAME] FROM [_SMDBA_].[_LOCATION_];", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            locations.Add(reader.GetString("NAME"));
                        }
                    }
                }
            }

            return locations;
        }

        public IList<string> GetUsers()
        {
            List<string> users = new List<string>();
            using (SqlConnection con = new SqlConnection(_trackitConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT [FNAME], [NAME] FROM [_SMDBA_].[_CUSTOMER_];", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add($"{reader.GetString("FNAME")} {reader.GetString("NAME")}");
                        }
                    }
                }
            }

            return users;
        }
         
        public IList<string> GetDepartments()
        {
            List<string> locations = new List<string>();
            using (SqlConnection con = new SqlConnection(_trackitConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT [NAME] FROM [_SMDBA_].[_DEPART_];", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            locations.Add(reader.GetString("NAME"));
                        }
                    }
                }
            }

            return locations;
        }



    }
}
