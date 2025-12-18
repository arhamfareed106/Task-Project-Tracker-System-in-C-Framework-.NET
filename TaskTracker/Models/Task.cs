using System;
using System.Text.Json.Serialization;

namespace TaskTracker.Models
{
    /// <summary>
    /// Represents a task in the task tracker system
    /// Implements IComparable for default sorting by creation date
    /// </summary>
    public class Task : IComparable<Task>
    {
        /// <summary>
        /// Gets the unique identifier of the task
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the task
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the task
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the due date of the task
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the priority of the task
        /// </summary>
        public Priority Priority { get; set; }

        /// <summary>
        /// Gets or sets the status of the task
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the assignee of the task
        /// </summary>
        public string Assignee { get; set; } = string.Empty;

        /// <summary>
        /// Gets the creation date of the task
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the completion date of the task (nullable)
        /// </summary>
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// Parameterless constructor for JSON serialization
        /// </summary>
        public Task()
        {
            // Used by JSON serializer
        }

        /// <summary>
        /// Constructor to create a new task with all required properties
        /// </summary>
        /// <param name="id">Unique identifier for the task</param>
        /// <param name="title">Title of the task</param>
        /// <param name="description">Description of the task</param>
        /// <param name="dueDate">Due date of the task</param>
        /// <param name="priority">Priority level of the task</param>
        /// <param name="assignee">Person assigned to the task</param>
        public Task(int id, string title, string description, DateTime dueDate, Priority priority, string assignee)
        {
            Id = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Status = Status.ToDo;
            Assignee = assignee;
            CreatedDate = DateTime.Now;
        }

        /// <summary>
        /// Updates the status of the task and sets the completion date if needed
        /// </summary>
        /// <param name="newStatus">The new status to set</param>
        public void UpdateStatus(Status newStatus)
        {
            Status = newStatus;
            if (newStatus == Status.Done)
            {
                CompletedDate = DateTime.Now;
            }
            else if (CompletedDate.HasValue)
            {
                CompletedDate = null;
            }
        }

        /// <summary>
        /// Checks if the task is overdue
        /// </summary>
        /// <returns>True if the task is overdue, false otherwise</returns>
        public bool IsOverdue()
        {
            return Status != Status.Done && DueDate < DateTime.Now;
        }

        /// <summary>
        /// Compares this task to another task based on creation date
        /// </summary>
        /// <param name="other">The task to compare to</param>
        /// <returns>A signed integer that indicates the relative values of this instance and other</returns>
        public int CompareTo(Task? other)
        {
            if (other == null) return 1;
            return CreatedDate.CompareTo(other.CreatedDate);
        }

        /// <summary>
        /// Returns a string representation of the task
        /// </summary>
        /// <returns>A formatted string containing task details</returns>
        public override string ToString()
        {
            return $"ID: {Id} | Title: {Title} | Due: {DueDate:yyyy-MM-dd} | Priority: {Priority} | Status: {Status} | Assignee: {Assignee}";
        }
    }
}