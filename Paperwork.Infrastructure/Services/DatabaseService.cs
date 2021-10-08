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
    public class DatabaseService : IDatabaseService
    {
        private readonly TrackitConfig _trackitConfig;
        public DatabaseService(IOptions<TrackitConfig> trackitConfig)
        {
            _trackitConfig = trackitConfig.Value;
        }

        public TrackitEquiptment GetTrackitItemInfo(string todNum)
        {
            List<TrackitEquiptment> trackitEquiptment = new List<TrackitEquiptment>();
            using (SqlConnection con = new SqlConnection(_trackitConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT TOP(1) * FROM dbo.vWORKSTATWITHUSER WHERE COMPNAME='{todNum}';", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            trackitEquiptment.Add(new TrackitEquiptment()
                            {
                                CurrentUser = !reader.IsDBNull("NAME") ? reader.GetString("NAME") : null,
                                ServiceTag = !reader.IsDBNull("COMPUTERSERVICETAG") ? reader.GetString("COMPUTERSERVICETAG") : null,
                                Department = !reader.IsDBNull("WORKSTAT_DEPT") ? reader.GetString("WORKSTAT_DEPT") : null,
                                Location = !reader.IsDBNull("WORKSTAT_LOCATION") ? reader.GetString("WORKSTAT_LOCATION") : null,
                                Type = !reader.IsDBNull("ID_2") ? reader.GetString("ID_2") : null,
                                Description = !reader.IsDBNull("ID_4") ? reader.GetString("ID_4") : null,
                                Price = !reader.IsDBNull("ID_3") ? reader.GetString("ID_3") : null,
                                TodNum = todNum,
                            });

                        }
                    }
                }
            }

            return trackitEquiptment.FirstOrDefault<TrackitEquiptment>();
        }

        public string GetWorkOrderNumber(string todNum)
        {
            List<TrackitEquiptment> trackitEquiptment = new List<TrackitEquiptment>();
            using (SqlConnection con = new SqlConnection(_trackitConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT TOP(1) WO_NUM FROM dbo.TASKS WHERE TASK LIKE '%{todNum}%' AND OPENDATE > DATEADD(day, -15, GETDATE()) ORDER BY WO_NUM DESC;", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetInt32("WO_NUM").ToString();
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
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT [LOCATION] FROM [dbo].[LOCATION];", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            locations.Add(reader.GetString("Location"));
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
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT [FULLNAME] FROM [dbo].[TIUSER];", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(reader.GetString("FULLNAME"));
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
                using (SqlCommand sqlCommand = new SqlCommand($"SELECT [DEPT] FROM [dbo].[DEPT];", con))
                {
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            locations.Add(reader.GetString("DEPT"));
                        }
                    }
                }
            }

            return locations;
        }



    }
}
