using APIClassRoom.Model;
using Microsoft.Data.SqlClient;

namespace APIClassRoom.Storage;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class ClassReservationStorage
{
    private readonly string _connectionString;

    public ClassReservationStorage(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<ClassReservation>> GetAllReservationsAsync()
    {
        var reservations = new List<ClassReservation>();
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query =
                "SELECT Id, IdClass, ClassName, Location, Department, Block, TeacherName, DateReserved, TimeSlot, Day, LevelId FROM ClassReservations";
            SqlCommand cmd = new SqlCommand(query, conn);
            await conn.OpenAsync();
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    reservations.Add(new ClassReservation
                    {
                        Id = reader.GetGuid(0),
                        IdClass = reader.GetInt32(1),
                        Department = reader.GetString(2),
                        Block = reader.GetString(3),
                        TeacherName = reader.GetString(4),
                        //DateReserved = reader.GetSqlDateTime(5),
                        TimeSlot = reader.GetString(6),
                        Day = reader.GetString(7),
                        LevelId = reader.GetInt32(8)
                    });
                }
            }
        }

        return reservations;
    }

    public async Task<bool> AddReservationAsync(ClassReservation reservation)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query =
                @"INSERT INTO ClassReservations (Id,IdClass, Department, Block, TeacherName, DateReserved, TimeSlot, Day, LevelId,Compensation,Description) 
                             VALUES (@Id,@IdClass, @Department, @Block, @TeacherName, @DateReserved, @TimeSlot, @Day, @LevelId,@Compensation,@Description)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
            cmd.Parameters.AddWithValue("@IdClass", reservation.IdClass);
            cmd.Parameters.AddWithValue("@Department", reservation.Department);
            cmd.Parameters.AddWithValue("@Block", reservation.Block);
            cmd.Parameters.AddWithValue("@TeacherName", reservation.TeacherName);
            cmd.Parameters.AddWithValue("@DateReserved", reservation.DateReserved);
            cmd.Parameters.AddWithValue("@TimeSlot", reservation.TimeSlot);
            cmd.Parameters.AddWithValue("@Day", reservation.Day);
            cmd.Parameters.AddWithValue("@LevelId", reservation.LevelId);
            cmd.Parameters.AddWithValue("@Compensation", reservation.Compensation);
            cmd.Parameters.AddWithValue("@Description", reservation.Description);


            await conn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }

    public async Task<bool> UpdateReservationAsync(ClassReservation reservation)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query =
                @"UPDATE ClassReservations SET IdClass = @IdClass, 
                             Department = @Department, Block = @Block, TeacherName = @TeacherName, DateReserved = @DateReserved, 
                             TimeSlot = @TimeSlot, Day = @Day, LevelId = @LevelId WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", reservation.Id);
            cmd.Parameters.AddWithValue("@IdClass", reservation.IdClass);
            cmd.Parameters.AddWithValue("@Department", reservation.Department);
            cmd.Parameters.AddWithValue("@Block", reservation.Block);
            cmd.Parameters.AddWithValue("@TeacherName", reservation.TeacherName);
            cmd.Parameters.AddWithValue("@DateReserved", reservation.DateReserved);
            cmd.Parameters.AddWithValue("@TimeSlot", reservation.TimeSlot);
            cmd.Parameters.AddWithValue("@Day", reservation.Day);
            cmd.Parameters.AddWithValue("@LevelId", reservation.LevelId);

            await conn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }

    public async Task<bool> UpdateStateClasse(ClassReservation reservation)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            // Validate column name
            HashSet<string> validColumns = new() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday" };
            if (!validColumns.Contains(reservation.Day))
            {
                throw new ArgumentException($"Invalid day column name: {reservation.Day}");
            }

           
            string query = $@"
            UPDATE Times 
            SET {reservation.Day} = @Compensation
            WHERE TimeSlot = @TimeSlot AND IdClass = @IdClass";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TimeSlot", reservation.TimeSlot);
                cmd.Parameters.AddWithValue("@IdClass", reservation.IdClass);
                cmd.Parameters.AddWithValue("@Compensation", reservation.Compensation);

                await conn.OpenAsync();
                int rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0; 
            }
        }
    }

    public async Task<DataTable> GetClassScheduleAsync()
    {
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
                ORDER BY T.IdClass, T.TimeSlot;";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                await conn.OpenAsync();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
    }

    public async Task<bool> DeleteReservationAsync(int id)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = "DELETE FROM ClassReservations WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await conn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
     public async Task<List<ClassReservation>> GetAllAsync()
    {
        var reservations = new List<ClassReservation>();

        string query = @"SELECT [Id], [IdClass], [Department], [Block], [TeacherName],
                                [DateReserved], [TimeSlot], [Day], [LevelId], 
                                [Compensation], [Description]
                         FROM [MyDb].[dbo].[ClassReservations]";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            await connection.OpenAsync();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    reservations.Add(new ClassReservation
                    {
                        Id = reader.GetGuid(0),
                        IdClass = reader.GetInt32(1),
                        Department = reader.GetString(2),
                        Block = reader.GetString(3),
                        TeacherName = reader.GetString(4),
                        DateReserved = DateOnly.FromDateTime(reader.GetDateTime(5)),
                        TimeSlot = reader.GetString(6),
                        Day = reader.GetString(7),
                        LevelId = reader.GetInt32(8),
                        Compensation = reader.GetString(9),
                        Description = reader.GetString(10)
                    });
                }
            }
        }

        return reservations;
    }


   public async Task<List<ClassReservation>> GetByIdAsync(string name)
   {
       var reservations = new List<ClassReservation>();
   
       string query = @"SELECT [Id], [IdClass], [Department], [Block], [TeacherName],
                               [DateReserved], [TimeSlot], [Day], [LevelId], 
                               [Compensation], [Description]
                        FROM [MyDb].[dbo].[ClassReservations]
                        WHERE [TeacherName] = @TeacherName";
   
       using (SqlConnection connection = new SqlConnection(_connectionString))
       using (SqlCommand command = new SqlCommand(query, connection))
       {
           command.Parameters.AddWithValue("@TeacherName", name);
           await connection.OpenAsync();
   
           using (SqlDataReader reader = await command.ExecuteReaderAsync())
           {
               while (await reader.ReadAsync())
               {
                   reservations.Add(new ClassReservation
                   {
                       Id = reader.GetGuid(0),
                       IdClass = reader.GetInt32(1),
                       Department = reader.GetString(2),
                       Block = reader.GetString(3),
                       TeacherName = reader.GetString(4),
                       DateReserved = DateOnly.FromDateTime(reader.GetDateTime(5)),
                       TimeSlot = reader.GetString(6),
                       Day = reader.GetString(7),
                       LevelId = reader.GetInt32(8),
                       Compensation = reader.GetString(9),
                       Description = reader.GetString(10)
                   });
               }
           }
       }
   
       return reservations;
   }

 
    public async Task<bool> DeleteAsync(Guid id)
    {
        string query = @"DELETE FROM [MyDb].[dbo].[ClassReservations] WHERE [Id] = @Id";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}