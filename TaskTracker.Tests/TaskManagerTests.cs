using Xunit;
using Moq;
using TaskTracker.Models;
using TaskTracker.Services;
using TaskTracker.Services.Interfaces;
using TaskTracker.Data.Repositories;
using TaskTracker.Data.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TaskTracker.Tests
{
    public class TaskManagerTests
    {
        private readonly Mock<TaskRepository> _mockRepository;
        private readonly Mock<ILogger> _mockLogger;
        private readonly TaskManager _taskManager;

        public TaskManagerTests()
        {
            _mockLogger = new Mock<ILogger>(MockBehavior.Loose);
            var mockFileHandler = new Mock<FileHandler>("test.json", _mockLogger.Object);
            _mockRepository = new Mock<TaskRepository>(mockFileHandler.Object, _mockLogger.Object);
            _taskManager = new TaskManager(_mockRepository.Object, _mockLogger.Object);
        }
        [Fact]
        public void GetAll_ShouldReturnAllTasks()
        {
            // Arrange
            var expectedTasks = new List<Task>
            {
                new Task(1, "Task 1", "Description 1", DateTime.Now.AddDays(1), Priority.High, "John"),
                new Task(2, "Task 2", "Description 2", DateTime.Now.AddDays(2), Priority.Medium, "Jane")
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(expectedTasks);

            // Act
            var result = _taskManager.GetAll();

            // Assert
            Assert.Equal(expectedTasks.Count, result.Count);
            _mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnTask()
        {
            // Arrange
            var taskId = 1;
            var expectedTask = new Task(taskId, "Test Task", "Test Description", DateTime.Now.AddDays(1), Priority.High, "John");
            _mockRepository.Setup(r => r.GetById(taskId)).Returns(expectedTask);

            // Act
            var result = _taskManager.GetById(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTask.Id, result.Id);
            _mockRepository.Verify(r => r.GetById(taskId), Times.Once);
        }

        [Fact]
        public void GetById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var taskId = 999;
            _mockRepository.Setup(r => r.GetById(taskId)).Throws(new TaskNotFoundException("Task not found"));

            // Act
            var result = _taskManager.GetById(taskId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Add_WithValidTask_ShouldCallRepositoryAdd()
        {
            // Arrange
            var task = new Task(1, "Valid Task", "Valid Description", DateTime.Now.AddDays(1), Priority.High, "John");

            // Act
            _taskManager.Add(task);

            // Assert
            _mockRepository.Verify(r => r.Add(task), Times.Once);
        }

        [Fact]
        public void Add_WithInvalidTask_ShouldThrowException()
        {
            // Arrange
            var task = new Task(1, "", "", DateTime.MinValue, Priority.High, "");

            // Act & Assert
            Assert.Throws<InvalidTaskDataException>(() => _taskManager.Add(task));
        }

        [Fact]
        public void Update_WithValidTask_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var task = new Task(1, "Updated Task", "Updated Description", DateTime.Now.AddDays(1), Priority.High, "John");

            // Act
            _taskManager.Update(task);

            // Assert
            _mockRepository.Verify(r => r.Update(task), Times.Once);
        }

        [Fact]
        public void Delete_WithValidId_ShouldCallRepositoryDelete()
        {
            // Arrange
            var taskId = 1;

            // Act
            _taskManager.Delete(taskId);

            // Assert
            _mockRepository.Verify(r => r.Delete(taskId), Times.Once);
        }

        [Fact]
        public void Search_WithValidTerm_ShouldReturnMatchingTasks()
        {
            // Arrange
            var searchTerm = "test";
            var tasks = new List<Task>
            {
                new Task(1, "Test Task", "Description 1", DateTime.Now.AddDays(1), Priority.High, "John"),
                new Task(2, "Another Task", "Test description", DateTime.Now.AddDays(2), Priority.Medium, "Jane")
            };
            _mockRepository.Setup(r => r.Search(searchTerm)).Returns(tasks);

            // Act
            var result = _taskManager.Search(searchTerm);

            // Assert
            Assert.Equal(tasks.Count, result.Count);
            _mockRepository.Verify(r => r.Search(searchTerm), Times.Once);
        }

        [Fact]
        public void Sort_WithValidStrategy_ShouldReturnSortedTasks()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task(1, "Task A", "Description 1", DateTime.Now.AddDays(2), Priority.Low, "John"),
                new Task(2, "Task B", "Description 2", DateTime.Now.AddDays(1), Priority.High, "Jane")
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(tasks);

            // Act
            List<Task> result = _taskManager.Sort("duedate", true);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.True(result[0].DueDate <= result[1].DueDate);
        }
    }
}