using APIClassRoom.Model;
using Microsoft.Data.SqlClient;

namespace APIClassRoom.Storage;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class TimeSlotStorage
{
    private readonly string _connectionString;

    public TimeSlotStorage(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<TimeSlot>> GetTimeSlotsByClassIdAsync(int classId)
    {
        var timeSlots = new List<TimeSlot>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            string query = "SELECT * FROM Times WHERE IdClass = @IdClass";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdClass", classId);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        timeSlots.Add(new TimeSlot
                        {
                            TimeSlotValue = reader.GetString(0),
                            IdClass = reader.GetInt32(1),
                            Sunday = reader.GetString(2),
                            Monday = reader.GetString(3),
                            Tuesday = reader.GetString(4),
                            Wednesday = reader.GetString(5),
                            Thursday = reader.GetString(6)
                        });
                    }
                }
            }
        }
        return timeSlots;
    }

    public async Task AddTimeSlotsForClassAsync(int classId)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            string query = @"
        INSERT INTO Times (TimeSlot, IdClass)
        SELECT TimeSlot, @IdClass FROM (VALUES
            ('8:00-9:30'),
            ('9:40-11:10'),
            ('11:20-13:10'),
            ('13:20-14:40'),
            ('14:50-16:20')
        ) AS T(TimeSlot)
        WHERE NOT EXISTS (
            SELECT 1 FROM Times WHERE TimeSlot = T.TimeSlot AND IdClass = @IdClass
        )";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdClass", classId);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }



}
