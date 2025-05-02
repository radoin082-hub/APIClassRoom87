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
     // Arrange
     var mockConnection = new Mock<IDbConnection>();
     var mockCommand = new Mock<IDbCommand>();
     var mockReader = new Mock<IDataReader>();
 
     // Mock the ConnectionString property
     mockConnection.Setup(conn => conn.ConnectionString).Returns("Server=RADOIN_CR;Database=MyDb;Trusted_Connection=True;MultipleActiveResultSets=true");
 
     mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
     mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(mockReader.Object);
 
     // Mock the reader to return only one row
     mockReader.SetupSequence(reader => reader.Read())
         .Returns(true)  // First row
         .Returns(false); // End of rows
 
     mockReader.Setup(reader => reader.GetInt32(0)).Returns(21);
     mockReader.Setup(reader => reader.GetString(1)).Returns("a");
     mockReader.Setup(reader => reader.GetString(2)).Returns("a");
     mockReader.Setup(reader => reader.GetString(3)).Returns("a");
     mockReader.Setup(reader => reader.GetString(4)).Returns("a");
 
     var classStorage = new ClassStorage(mockConnection.Object);
 
     // Act
     var result = await classStorage.GetAllClassesAsync();
 
     // Assert
     Assert.NotNull(result);
    //Assert.Single(result); 
     Assert.Equal("a", result[0].Name);
     Assert.NotEqual(0, result[0].IdClass);
     Assert.True(result[0].IdClass > 0);
 }
}