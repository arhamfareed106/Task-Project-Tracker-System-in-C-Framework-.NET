using Xunit;
using Moq;
using TaskTracker.Services;
using TaskTracker.Services.Interfaces;
using TaskTracker.Models;
using TaskTracker.Data.Exceptions;
using System.Text.Json;
using System.IO;

namespace TaskTracker.Tests
{
    public class FileHandlerTests : IDisposable
    {
        private readonly string _testFilePath;
        private readonly Mock<ILogger> _mockLogger;
        private readonly FileHandler _fileHandler;

        public FileHandlerTests()
        {
            _testFilePath = Path.Combine(Path.GetTempPath(), $"tasks_test_{Guid.NewGuid()}.json");
            _mockLogger = new Mock<ILogger>(MockBehavior.Loose);
            _fileHandler = new FileHandler(_testFilePath, _mockLogger.Object);
        }        public void Dispose()
        {
            // Clean up test file
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [Fact]
        public void ReadTasks_WhenFileDoesNotExist_ShouldCreateNewFileAndReturnEmptyList()
        {
            // Act
            var result = _fileHandler.ReadTasks();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ReadTasks_WithValidJson_ShouldReturnTasks()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task(1, "Test Task 1", "Description 1", DateTime.Now.AddDays(1), Priority.High, "John"),
                new Task(2, "Test Task 2", "Description 2", DateTime.Now.AddDays(2), Priority.Medium, "Jane")
            };

            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_testFilePath, json);

            // Act
            var result = _fileHandler.ReadTasks();

            // Assert
            Assert.Equal(tasks.Count, result.Count);
            Assert.Equal(tasks[0].Id, result[0].Id);
            Assert.Equal(tasks[1].Id, result[1].Id);
        }

        [Fact]
        public void ReadTasks_WithInvalidJson_ShouldThrowInvalidTaskDataException()
        {
            // Arrange
            File.WriteAllText(_testFilePath, "invalid json content");

            // Act & Assert
            Assert.Throws<InvalidTaskDataException>(() => _fileHandler.ReadTasks());
        }

        [Fact]
        public void WriteTasks_WithValidTasks_ShouldWriteToFile()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task(1, "Test Task", "Description", DateTime.Now.AddDays(1), Priority.High, "John")
            };

            // Act
            _fileHandler.WriteTasks(tasks);

            // Assert
            Assert.True(File.Exists(_testFilePath));
            var json = File.ReadAllText(_testFilePath);
            Assert.NotEmpty(json);
        }
    }
}