using APIClassRoom.Model;
using Microsoft.Data.SqlClient;

namespace APIClassRoom.Storage;

using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ClassScheduleStorage
{
    private readonly string _connectionString;

    public ClassScheduleStorage(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<ClassScheduleModel>> GetClassScheduleAsync(int idClass)
    {
        List<ClassScheduleModel> schedules = new List<ClassScheduleModel>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = @"
            SELECT 
                T.TimeSlot,
                T.IdClass,
                C.Name,
                C.Block,
                C.Department,
                T.Sunday,
                T.Monday,
                T.Tuesday,
                T.Wednesday,
                T.Thursday
            FROM [MyDb].[dbo].[Times] AS T
            JOIN [MyDb].[dbo].[Classes] AS C ON T.IdClass = C.IdClass
            WHERE T.IdClass = @IdClass
            ORDER BY T.TimeSlot;";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdClass", idClass);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        schedules.Add(new ClassScheduleModel
                        {
                            TimeSlot = reader["TimeSlot"].ToString(),
                            IdClass = (int)reader["IdClass"],
                            Name = reader["Name"].ToString(),
                            Block = reader["Block"].ToString(),
                            Department = reader["Department"].ToString(),
                            Sunday = (string)reader["Sunday"],
                            Monday = (string)reader["Monday"],
                            Tuesday = (string)reader["Tuesday"],
                            Wednesday = (string)reader["Wednesday"],
                            Thursday = (string)reader["Thursday"]
                        });
                    }
                }
            }
        }
        return schedules;
    }
    
    
    

}


