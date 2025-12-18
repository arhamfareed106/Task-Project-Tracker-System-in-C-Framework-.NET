using Xunit;
using TaskTracker.Models;
using TaskTracker.Utilities;

namespace TaskTracker.Tests
{
    public class ExtensionMethodTests
    {
        [Fact]
        public void IsDueWithin_WithTaskDueToday_ShouldReturnTrue()
        {
            // Arrange
            var task = new TaskTracker.Models.Task(1, "Test Task", "Description", DateTime.Today, TaskTracker.Models.Priority.High, "John");

            // Act
            var result = task.IsDueWithin(0);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDueWithin_WithTaskDueInFuture_ShouldReturnFalse()
        {
            // Arrange
            var task = new TaskTracker.Models.Task(1, "Test Task", "Description", DateTime.Today.AddDays(5), TaskTracker.Models.Priority.High, "John");

            // Act
            var result = task.IsDueWithin(2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDueWithin_WithCompletedTask_ShouldReturnFalse()
        {
            // Arrange
            var task = new TaskTracker.Models.Task(1, "Test Task", "Description", DateTime.Today, TaskTracker.Models.Priority.High, "John");
            task.UpdateStatus(TaskTracker.Models.Status.Done);

            // Act
            var result = task.IsDueWithin(0);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Priority_ToColoredString_ReturnsCorrectString()
        {
            // Arrange
            var highPriority = Priority.High;
            var mediumPriority = Priority.Medium;
            var lowPriority = Priority.Low;

            // Act
            var highString = highPriority.ToColoredString();
            var mediumString = mediumPriority.ToColoredString();
            var lowString = lowPriority.ToColoredString();

            // Assert
            Assert.Equal("[HIGH]", highString);
            Assert.Equal("[MED]", mediumString);
            Assert.Equal("[LOW]", lowString);
        }

        [Fact]
        public void Status_ToColoredString_ReturnsCorrectString()
        {
            // Arrange
            var todoStatus = Status.ToDo;
            var inProgressStatus = Status.InProgress;
            var doneStatus = Status.Done;

            // Act
            var todoString = todoStatus.ToColoredString();
            var inProgressString = inProgressStatus.ToColoredString();
            var doneString = doneStatus.ToColoredString();

            // Assert
            Assert.Equal("[TODO]", todoString);
            Assert.Equal("[IN PROGRESS]", inProgressString);
            Assert.Equal("[DONE]", doneString);
        }
    }
}