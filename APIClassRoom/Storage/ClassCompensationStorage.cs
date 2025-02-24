using APIClassRoom.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIClassRoom.Storage
{
    public class ClassCompensationStorage
    {
        private readonly string _connectionString;

        public ClassCompensationStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<ClassReservationDto>> GetReservationsByLevelIdAsync(int levelId)
        {
            List<ClassReservationDto> reservations = new List<ClassReservationDto>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = @"
                    SELECT 
                        cr.[Id], 
                        cr.[IdClass], 
                        c.[Name] AS ClassName, 
                        cr.[Department], 
                        cr.[Block], 
                        cr.[TeacherName], 
                        cr.[DateReserved], 
                        cr.[TimeSlot], 
                        cr.[Day], 
                        cr.[LevelId], 
                        cr.[Compensation], 
                        cr.[Description]
                    FROM 
                        [MyDb].[dbo].[ClassReservations] cr
                    JOIN 
                        [MyDb].[dbo].[Classes] c ON cr.[IdClass] = c.[IdClass]
                    WHERE 
                        cr.[LevelId] = @LevelId;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LevelId", levelId);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync()) // Loop through all rows
                        {
                            var reservation = new ClassReservationDto
                            {
                                Id = reader.GetGuid(0),
                                IdClass = reader.GetInt32(1),
                                ClassName = reader.GetString(2),
                                Department = reader.GetString(3),
                                Block = reader.GetString(4),
                                TeacherName = reader.GetString(5),
                                DateReserved = reader.GetDateTime(6),
                                TimeSlot = reader.GetString(7),
                                Day = reader.GetString(8),
                                LevelId = reader.GetInt32(9),
                                Compensation = reader.IsDBNull(10) ? null : reader.GetString(10),
                                Description = reader.IsDBNull(11) ? null : reader.GetString(11)
                            };

                            reservations.Add(reservation);
                        }
                    }
                }
            }

            return reservations;
        }
    }
}
