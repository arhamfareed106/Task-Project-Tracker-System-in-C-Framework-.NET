using Xunit;
using TaskTracker.Models;
using TaskTracker.Services.SortStrategies;

namespace TaskTracker.Tests
{
    public class SortStrategyTests
    {
        [Fact]
        public void SortByDueDate_Ascending_ShouldSortCorrectly()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task(1, "Task C", "Desc C", new DateTime(2023, 12, 31), Priority.High, "John"),
                new Task(2, "Task A", "Desc A", new DateTime(2023, 10, 15), Priority.Low, "Jane"),
                new Task(3, "Task B", "Desc B", new DateTime(2023, 11, 20), Priority.Medium, "Bob")
            };

            var strategy = new SortByDueDate();

            // Act
            var sortedTasks = strategy.Sort(tasks, true);

            // Assert
            Assert.Equal(3, sortedTasks.Count);
            Assert.Equal("Task A", sortedTasks[0].Title); // Oct 15
            Assert.Equal("Task B", sortedTasks[1].Title); // Nov 20
            Assert.Equal("Task C", sortedTasks[2].Title); // Dec 31
        }

        [Fact]
        public void SortByDueDate_Descending_ShouldSortCorrectly()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task(1, "Task C", "Desc C", new DateTime(2023, 12, 31), Priority.High, "John"),
                new Task(2, "Task A", "Desc A", new DateTime(2023, 10, 15), Priority.Low, "Jane"),
                new Task(3, "Task B", "Desc B", new DateTime(2023, 11, 20), Priority.Medium, "Bob")
            };

            var strategy = new SortByDueDate();

            // Act
            var sortedTasks = strategy.Sort(tasks, false);

            // Assert
            Assert.Equal(3, sortedTasks.Count);
            Assert.Equal("Task C", sortedTasks[0].Title); // Dec 31
            Assert.Equal("Task B", sortedTasks[1].Title); // Nov 20
            Assert.Equal("Task A", sortedTasks[2].Title); // Oct 15
        }

        [Fact]
        public void SortByPriority_Ascending_ShouldSortCorrectly()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task(1, "Task C", "Desc C", DateTime.Now, Priority.High, "John"),
                new Task(2, "Task A", "Desc A", DateTime.Now, Priority.Low, "Jane"),
                new Task(3, "Task B", "Desc B", DateTime.Now, Priority.Medium, "Bob")
            };

            var strategy = new SortByPriority();

            // Act
            var sortedTasks = strategy.Sort(tasks, true);

            // Assert
            Assert.Equal(3, sortedTasks.Count);
            Assert.Equal("Task A", sortedTasks[0].Title); // Low
            Assert.Equal("Task B", sortedTasks[1].Title); // Medium
            Assert.Equal("Task C", sortedTasks[2].Title); // High
        }

        [Fact]
        public void SortByPriority_Descending_ShouldSortCorrectly()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task(1, "Task C", "Desc C", DateTime.Now, Priority.High, "John"),
                new Task(2, "Task A", "Desc A", DateTime.Now, Priority.Low, "Jane"),
                new Task(3, "Task B", "Desc B", DateTime.Now, Priority.Medium, "Bob")
            };

            var strategy = new SortByPriority();

            // Act
            var sortedTasks = strategy.Sort(tasks, false);

            // Assert
            Assert.Equal(3, sortedTasks.Count);
            Assert.Equal("Task C", sortedTasks[0].Title); // High
            Assert.Equal("Task B", sortedTasks[1].Title); // Medium
            Assert.Equal("Task A", sortedTasks[2].Title); // Low
        }

        [Fact]
        public void SortByAssignee_Ascending_ShouldSortCorrectly()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task(1, "Task C", "Desc C", DateTime.Now, Priority.High, "John"),
                new Task(2, "Task A", "Desc A", DateTime.Now, Priority.Low, "Alice"),
                new Task(3, "Task B", "Desc B", DateTime.Now, Priority.Medium, "Bob")
            };

            var strategy = new SortByAssignee();

            // Act
            var sortedTasks = strategy.Sort(tasks, true);

            // Assert
            Assert.Equal(3, sortedTasks.Count);
            Assert.Equal("Task A", sortedTasks[0].Title); // Alice
            Assert.Equal("Task B", sortedTasks[1].Title); // Bob
            Assert.Equal("Task C", sortedTasks[2].Title); // John
        }

        [Fact]
        public void SortByAssignee_Descending_ShouldSortCorrectly()
        {
            // Arrange
            var tasks = new List<Task>
            {
                new Task(1, "Task C", "Desc C", DateTime.Now, Priority.High, "John"),
                new Task(2, "Task A", "Desc A", DateTime.Now, Priority.Low, "Alice"),
                new Task(3, "Task B", "Desc B", DateTime.Now, Priority.Medium, "Bob")
            };

            var strategy = new SortByAssignee();

            // Act
            var sortedTasks = strategy.Sort(tasks, false);

            // Assert
            Assert.Equal(3, sortedTasks.Count);
            Assert.Equal("Task C", sortedTasks[0].Title); // John
            Assert.Equal("Task B", sortedTasks[1].Title); // Bob
            Assert.Equal("Task A", sortedTasks[2].Title); // Alice
        }
    }
}