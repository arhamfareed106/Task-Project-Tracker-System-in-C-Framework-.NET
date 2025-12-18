using Xunit;
using Moq;
using TaskTracker.Models;
using TaskTracker.Services;
using TaskTracker.Services.Interfaces;
using TaskTracker.Data.Repositories;
using TaskTracker.Data.Exceptions;
using System.Collections.Generic;
using System;
namespace TaskTracker.Tests
{
    public class SearchAndSortTests
    {
        private readonly Mock<TaskRepository> _mockRepository;
        private readonly Mock<ILogger> _mockLogger;
        private readonly TaskManager _taskManager;

        public SearchAndSortTests()
        {
            _mockLogger = new Mock<ILogger>(MockBehavior.Loose);
            var mockFileHandler = new Mock<FileHandler>("test.json", _mockLogger.Object);
            _mockRepository = new Mock<TaskRepository>(mockFileHandler.Object, _mockLogger.Object);
            _taskManager = new TaskManager(_mockRepository.Object, _mockLogger.Object);
        }
        [Fact]
        public void LinearSearch_WithMatchingTerm_ReturnsMatchingTasks()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task(1, "Important Task", "Description 1", DateTime.Now.AddDays(1), Priority.High, "John"),
                new Task(2, "Regular Task", "Description 2", DateTime.Now.AddDays(2), Priority.Medium, "Jane"),
                new Task(3, "Another Task", "Description 3", DateTime.Now.AddDays(3), Priority.Low, "John")
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(tasks);

            // Act
            var result = _taskManager.LinearSearch("Important");

            // Assert
            Assert.Single(result);
            Assert.Equal("Important Task", result[0].Title);
        }

        [Fact]
        public void LinearSearch_WithNoMatchingTerm_ReturnsEmptyList()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task(1, "Test Task", "Description", DateTime.Now.AddDays(1), Priority.High, "John")
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(tasks);

            // Act
            var result = _taskManager.LinearSearch("NonExistent");

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void LinearSearch_WithEmptyTerm_ReturnsAllTasks()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task(1, "Task 1", "Description 1", DateTime.Now.AddDays(1), Priority.High, "John"),
                new Task(2, "Task 2", "Description 2", DateTime.Now.AddDays(2), Priority.Medium, "Jane")
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(tasks);

            // Act
            var result = _taskManager.LinearSearch("");

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void LinqSort_ByDueDateAscending_ReturnsCorrectlySortedTasks()
        {
            // Arrange
            var earlyTask = new Task(1, "Early Task", "Description 1", DateTime.Now.AddDays(1), Priority.High, "John");
            var lateTask = new Task(2, "Late Task", "Description 2", DateTime.Now.AddDays(5), Priority.Medium, "Jane");
            var tasks = new List<Task> { lateTask, earlyTask }; // Intentionally unsorted
            _mockRepository.Setup(r => r.GetAll()).Returns(tasks);

            // Act
            var result = _taskManager.LinqSort("duedate", true);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Early Task", result[0].Title); // Should be first due to earlier date
            Assert.Equal("Late Task", result[1].Title);  // Should be second due to later date
        }

        [Fact]
        public void LinqSort_ByPriorityDescending_ReturnsCorrectlySortedTasks()
        {
            // Arrange
            var highPriorityTask = new Task(1, "High Priority", "Description 1", DateTime.Now.AddDays(1), Priority.High, "John");
            var lowPriorityTask = new Task(2, "Low Priority", "Description 2", DateTime.Now.AddDays(2), Priority.Low, "Jane");
            var tasks = new List<Task> { lowPriorityTask, highPriorityTask }; // Intentionally unsorted
            _mockRepository.Setup(r => r.GetAll()).Returns(tasks);

            // Act
            var result = _taskManager.LinqSort("priority", false);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("High Priority", result[0].Title); // Should be first due to higher priority
            Assert.Equal("Low Priority", result[1].Title);  // Should be second due to lower priority
        }

        [Fact]
        public void LinqSort_WithInvalidSortField_ReturnsUnsortedTasks()
        {
            // Arrange
            var task1 = new Task(1, "Task 1", "Description 1", DateTime.Now.AddDays(1), Priority.High, "John");
            var task2 = new Task(2, "Task 2", "Description 2", DateTime.Now.AddDays(2), Priority.Medium, "Jane");
            var tasks = new List<Task> { task2, task1 };
            _mockRepository.Setup(r => r.GetAll()).Returns(tasks);

            // Act
            var result = _taskManager.LinqSort("invalidfield", true);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Task 2", result[0].Title); // Should remain in original order
            Assert.Equal("Task 1", result[1].Title);
        }
    }
}