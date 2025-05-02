namespace APIClassRoom.Test;

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using APIClassRoom.Model;
using APIClassRoom.Storage;
using Moq;
using Xunit;

public class ClassStorageTests
{
    [Fact]
    public async Task GetAllClassesAsync_ReturnsListOfClasses()
    {
        
        var mockConnection = new Mock<IDbConnection>();
        var mockCommand = new Mock<IDbCommand>();
        var mockReader = new Mock<IDataReader>();

        mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
        mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(mockReader.Object);

        mockReader.SetupSequence(reader => reader.Read())
            .Returns(true) // First row
            .Returns(false); // End of rows

        mockReader.Setup(reader => reader.GetInt32(0)).Returns(1);
        mockReader.Setup(reader => reader.GetString(1)).Returns("Class A");
        mockReader.Setup(reader => reader.GetString(2)).Returns("Room 101");
        mockReader.Setup(reader => reader.GetString(3)).Returns("Block A");
        mockReader.Setup(reader => reader.GetString(4)).Returns("Science");

        var classStorage = new ClassStorage("Server=RADOIN_CR;Database=MyDb;Trusted_Connection=True;MultipleActiveResultSets=true");

        // Act
        var result = await classStorage.GetAllClassesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Class A", result[0].Name);
    }
}