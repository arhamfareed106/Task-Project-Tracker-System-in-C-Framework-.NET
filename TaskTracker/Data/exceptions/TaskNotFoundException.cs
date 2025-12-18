using System;

namespace TaskTracker.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when a task is not found
    /// </summary>
    public class TaskNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the TaskNotFoundException class
        /// </summary>
        public TaskNotFoundException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TaskNotFoundException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public TaskNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TaskNotFoundException class with a specified error message 
        /// and a reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public TaskNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}