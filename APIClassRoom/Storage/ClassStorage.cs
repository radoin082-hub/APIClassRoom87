using APIClassRoom.Model;
using Microsoft.Data.SqlClient;

namespace APIClassRoom.Storage;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class ClassStorage
{
    private readonly IDbConnection _connection;

    public ClassStorage(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<ClassRoom>> GetAllClassesAsync()
    {
        var classes = new List<ClassRoom>();

        using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
        {
            await conn.OpenAsync();
            string query = "SELECT * FROM Classes";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    classes.Add(new ClassRoom
                    {
                        IdClass = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Localisation = reader.GetString(2),
                        Block = reader.GetString(3),
                        Department = reader.GetString(4)
                    });
                }
            }
        }
        return classes;
    }

    public async Task<int> AddClassAsync(ClassRoom newClass)
    {
        int newClassId = 0;

        using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
        {
            await conn.OpenAsync();
            var query = @"
            INSERT INTO Classes (Name, Localisation, Block, Department) 
            VALUES (@Name, @Localisation, @Block, @Department);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";  // Get last inserted ID

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", newClass.Name);
                cmd.Parameters.AddWithValue("@Localisation", newClass.Localisation);
                cmd.Parameters.AddWithValue("@Block", newClass.Block);
                cmd.Parameters.AddWithValue("@Department", newClass.Department);

                newClassId = (int)await cmd.ExecuteScalarAsync();
            }
        }

        return newClassId;
    }


    public async Task<bool> DeleteClassAsync(int id)
    {
        using (SqlConnection conn = new SqlConnection(_connection.ConnectionString))
        {
            await conn.OpenAsync();
            string query = "DELETE FROM Classes WHERE IdClass = @Id";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                var r=await cmd.ExecuteNonQueryAsync();
                return r > 0 ? true : false;
            }
        }
    }
}
