using Xunit;
using TaskTracker.Models;
using TaskTracker.Utilities;
using TaskTracker.Data.Exceptions;

namespace TaskTracker.Tests
{
    public class ValidatorTests
    {
        [Fact]
        public void ValidateTask_WithNullTask_ThrowsInvalidTaskDataException()
        {
            // Arrange
            Task? task = null;

            // Act & Assert
            Assert.Throws<InvalidTaskDataException>(() => Validator.ValidateTask(task!));
        }

        [Fact]
        public void ValidateTask_WithEmptyTitle_ThrowsInvalidTaskDataException()
        {
            // Arrange
            var task = new Task(1, "", "Description", DateTime.Now.AddDays(1), Priority.High, "John");

            // Act & Assert
            Assert.Throws<InvalidTaskDataException>(() => Validator.ValidateTask(task));
        }

        [Fact]
        public void ValidateTask_WithEmptyAssignee_ThrowsInvalidTaskDataException()
        {
            // Arrange
            var task = new Task(1, "Title", "Description", DateTime.Now.AddDays(1), Priority.High, "");

            // Act & Assert
            Assert.Throws<InvalidTaskDataException>(() => Validator.ValidateTask(task));
        }

        [Fact]
        public void ValidateTask_WithUnsetDueDate_ThrowsInvalidTaskDataException()
        {
            // Arrange
            var task = new Task(1, "Title", "Description", DateTime.MinValue, Priority.High, "John");

            // Act & Assert
            Assert.Throws<InvalidTaskDataException>(() => Validator.ValidateTask(task));
        }

        [Fact]
        public void ValidateTask_WithPastDueDate_ThrowsInvalidTaskDataException()
        {
            // Arrange
            var task = new Task(1, "Title", "Description", DateTime.Now.AddDays(-1), Priority.High, "John");

            // Act & Assert
            Assert.Throws<InvalidTaskDataException>(() => Validator.ValidateTask(task));
        }

        [Fact]
        public void ValidateTask_WithValidTask_DoesNotThrow()
        {
            // Arrange
            var task = new Task(1, "Title", "Description", DateTime.Now.AddDays(1), Priority.High, "John");

            // Act & Assert
            var exception = Record.Exception(() => Validator.ValidateTask(task));
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateId_WithZeroId_ThrowsInvalidTaskDataException()
        {
            // Arrange
            var id = 0;

            // Act & Assert
            Assert.Throws<InvalidTaskDataException>(() => Validator.ValidateId(id));
        }

        [Fact]
        public void ValidateId_WithNegativeId_ThrowsInvalidTaskDataException()
        {
            // Arrange
            var id = -1;

            // Act & Assert
            Assert.Throws<InvalidTaskDataException>(() => Validator.ValidateId(id));
        }

        [Fact]
        public void ValidateId_WithPositiveId_DoesNotThrow()
        {
            // Arrange
            var id = 1;

            // Act & Assert
            var exception = Record.Exception(() => Validator.ValidateId(id));
            Assert.Null(exception);
        }
    }
}