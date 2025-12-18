using System;

namespace TaskTracker.Data.Exceptions
{
    /// <summary>
    /// Exception thrown when task data is invalid
    /// </summary>
    public class InvalidTaskDataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidTaskDataException class
        /// </summary>
        public InvalidTaskDataException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidTaskDataException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public InvalidTaskDataException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidTaskDataException class with a specified error message 
        /// and a reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public InvalidTaskDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}