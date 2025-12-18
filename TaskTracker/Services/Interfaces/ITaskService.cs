using System.Collections.Generic;
using TaskTracker.Models;

namespace TaskTracker.Services.Interfaces
{
    /// <summary>
    /// Interface for task management services
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Gets all tasks
        /// </summary>
        /// <returns>A list of all tasks</returns>
        List<Task> GetAll();

        /// <summary>
        /// Gets a task by its ID
        /// </summary>
        /// <param name="id">The ID of the task to retrieve</param>
        /// <returns>The task with the specified ID, or null if not found</returns>
        Task? GetById(int id);

        /// <summary>
        /// Adds a new task
        /// </summary>
        /// <param name="task">The task to add</param>
        void Add(Task task);

        /// <summary>
        /// Updates an existing task
        /// </summary>
        /// <param name="task">The task with updated information</param>
        void Update(Task task);

        /// <summary>
        /// Deletes a task by its ID
        /// </summary>
        /// <param name="id">The ID of the task to delete</param>
        void Delete(int id);

        /// <summary>
        /// Searches for tasks based on a search term
        /// </summary>
        /// <param name="searchTerm">The term to search for in task properties</param>
        /// <returns>A list of tasks matching the search criteria</returns>
        List<Task> Search(string searchTerm);

        /// <summary>
        /// Sorts tasks using a specified sorting strategy
        /// </summary>
        /// <param name="strategyName">The name of the sorting strategy to use</param>
        /// <param name="ascending">Whether to sort in ascending order</param>
        /// <returns>A sorted list of tasks</returns>
        List<Task> Sort(string strategyName, bool ascending = true);

        /// <summary>
        /// Searches for tasks based on a search term using linear search algorithm
        /// </summary>
        /// <param name="searchTerm">The term to search for in task properties</param>
        /// <returns>A list of tasks matching the search criteria</returns>
        List<Task> LinearSearch(string searchTerm);

        /// <summary>
        /// Sorts tasks using built-in LINQ sorting for comparison
        /// </summary>
        /// <param name="sortBy">The field to sort by (duedate, priority, assignee, createddate)</param>
        /// <param name="ascending">Whether to sort in ascending order</param>
        /// <returns>A sorted list of tasks</returns>
        List<Task> LinqSort(string sortBy, bool ascending = true);
    }
}