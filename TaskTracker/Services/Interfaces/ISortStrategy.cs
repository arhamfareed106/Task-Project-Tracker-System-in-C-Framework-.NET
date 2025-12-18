using System.Collections.Generic;
using TaskTracker.Models;

namespace TaskTracker.Services.Interfaces
{
    /// <summary>
    /// Interface for sorting strategies
    /// </summary>
    public interface ISortStrategy
    {
        /// <summary>
        /// Sorts a list of tasks
        /// </summary>
        /// <param name="tasks">The list of tasks to sort</param>
        /// <param name="ascending">Whether to sort in ascending order</param>
        /// <returns>A sorted list of tasks</returns>
        List<Task> Sort(List<Task> tasks, bool ascending = true);
    }
}