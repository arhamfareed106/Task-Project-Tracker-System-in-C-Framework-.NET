using System;
using TaskTracker.Models;

namespace TaskTracker.Utilities
{
    /// <summary>
    /// Extension methods for Task objects
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Checks if a task is due within a specified number of days
        /// </summary>
        /// <param name="task">The task to check</param>
        /// <param name="days">Number of days to check within</param>
        /// <returns>True if the task is due within the specified days, false otherwise</returns>
        public static bool IsDueWithin(this TaskTracker.Models.Task task, int days)
        {
            if (task.Status == TaskTracker.Models.Status.Done)
                return false;
                
            var dueDate = task.DueDate.Date;
            var currentDate = DateTime.Now.Date;
            var difference = dueDate - currentDate;
            
            return difference.TotalDays >= 0 && difference.TotalDays <= days;
        }

        /// <summary>
        /// Gets a string representation of the task's priority with color coding
        /// </summary>
        /// <param name="priority">The priority to convert</param>
        /// <returns>A string representation of the priority</returns>
        public static string ToColoredString(this Priority priority)
        {
            return priority switch
            {
                Priority.High => "[HIGH]",
                Priority.Medium => "[MED]",
                Priority.Low => "[LOW]",
                _ => "[UNK]"
            };
        }

        /// <summary>
        /// Gets a string representation of the task's status with color coding
        /// </summary>
        /// <param name="status">The status to convert</param>
        /// <returns>A string representation of the status</returns>
        public static string ToColoredString(this Status status)
        {
            return status switch
            {
                Status.ToDo => "[TODO]",
                Status.InProgress => "[IN PROGRESS]",
                Status.Done => "[DONE]",
                _ => "[UNKNOWN]"
            };
        }
    }
}