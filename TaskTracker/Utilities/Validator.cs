using TaskTracker.Models;
using TaskTracker.Data.Exceptions;

namespace TaskTracker.Utilities
{
    /// <summary>
    /// Utility class for validating task data
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Validates a task object
        /// </summary>
        /// <param name="task">The task to validate</param>
        /// <exception cref="InvalidTaskDataException">Thrown when task data is invalid</exception>
        public static void ValidateTask(Task task)
        {
            if (task == null)
            {
                throw new InvalidTaskDataException("Task cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new InvalidTaskDataException("Task title cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(task.Assignee))
            {
                throw new InvalidTaskDataException("Task assignee cannot be null or empty.");
            }

            if (task.DueDate == DateTime.MinValue)
            {
                throw new InvalidTaskDataException("Task due date is not set.");
            }

            if (task.DueDate < DateTime.Today)
            {
                throw new InvalidTaskDataException("Task due date cannot be in the past.");
            }
        }

        /// <summary>
        /// Validates a task ID
        /// </summary>
        /// <param name="id">The ID to validate</param>
        /// <exception cref="InvalidTaskDataException">Thrown when the ID is invalid</exception>
        public static void ValidateId(int id)
        {
            if (id <= 0)
            {
                throw new InvalidTaskDataException("Task ID must be a positive integer.");
            }
        }
    }
}