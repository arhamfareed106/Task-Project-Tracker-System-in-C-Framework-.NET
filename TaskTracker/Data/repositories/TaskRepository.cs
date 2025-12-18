using System;
using System.Collections.Generic;
using System.Linq;
using TaskTracker.Models;
using TaskTracker.Services;
using TaskTracker.Services.Interfaces;
using TaskTracker.Data.Exceptions;

namespace TaskTracker.Data.Repositories
{
    /// <summary>
    /// Repository for managing task data with persistence
    /// </summary>
    public class TaskRepository
    {
        private readonly FileHandler _fileHandler;
        private List<Task> _tasks;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the TaskRepository class
        /// </summary>
        /// <param name="fileHandler">The file handler for persistence</param>
        /// <param name="logger">The logger instance</param>
        public TaskRepository(FileHandler fileHandler, ILogger logger)
        {
            _fileHandler = fileHandler;
            _logger = logger;
            _tasks = new List<Task>();
            LoadTasks();
        }

        /// <summary>
        /// Loads tasks from the data source
        /// </summary>
        private void LoadTasks()
        {
            try
            {
                var tasks = _fileHandler.ReadTasks();
                _tasks = tasks;
                _logger.LogInfo($"Loaded {_tasks.Count} tasks from repository");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to load tasks: {ex.Message}");
                _tasks = new List<Task>(); // Initialize with empty list as fallback
            }
        }

        /// <summary>
        /// Saves tasks to the data source
        /// </summary>
        private void SaveTasks()
        {
            try
            {
                _fileHandler.WriteTasks(_tasks.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save tasks: {ex.Message}");
                throw; // Re-throw to let caller handle
            }
        }

        /// <summary>
        /// Gets all tasks
        /// </summary>
        /// <returns>A list of all tasks</returns>
        public virtual List<Task> GetAll()
        {
            return new List<Task>(_tasks);
        }

        /// <summary>
        /// Gets a task by its ID
        /// </summary>
        /// <param name="id">The ID of the task to retrieve</param>
        /// <returns>The task with the specified ID</returns>
        /// <exception cref="TaskNotFoundException">Thrown when task with specified ID is not found</exception>
        public virtual Task GetById(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                throw new TaskNotFoundException($"Task with ID {id} not found.");
            }
            return task;
        }

        /// <summary>
        /// Adds a new task
        /// </summary>
        /// <param name="task">The task to add</param>
        public virtual void Add(Task task)
        {
            _tasks.Add(task);
            SaveTasks();
            _logger.LogInfo($"Added task with ID {task.Id}");
        }

        /// <summary>
        /// Updates an existing task
        /// </summary>
        /// <param name="task">The task with updated information</param>
        /// <exception cref="TaskNotFoundException">Thrown when task with specified ID is not found</exception>
        public virtual void Update(Task task)
        {
            var index = _tasks.FindIndex(t => t.Id == task.Id);
            if (index == -1)
            {
                throw new TaskNotFoundException($"Task with ID {task.Id} not found.");
            }

            _tasks[index] = task;
            SaveTasks();
            _logger.LogInfo($"Updated task with ID {task.Id}");
        }

        /// <summary>
        /// Deletes a task by its ID
        /// </summary>
        /// <param name="id">The ID of the task to delete</param>
        /// <exception cref="TaskNotFoundException">Thrown when task with specified ID is not found</exception>
        public virtual void Delete(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                throw new TaskNotFoundException($"Task with ID {id} not found.");
            }

            _tasks.Remove(task);
            SaveTasks();
            _logger.LogInfo($"Deleted task with ID {id}");
        }

        /// <summary>
        /// Searches for tasks that match the search term
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>A list of tasks matching the search criteria</returns>
        public virtual List<Task> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Task>(_tasks);
            }

            var lowerSearchTerm = searchTerm.ToLower();
            return _tasks.Where(task =>
                task.Id.ToString().Contains(searchTerm) ||
                task.Title.ToLower().Contains(lowerSearchTerm) ||
                task.Assignee.ToLower().Contains(lowerSearchTerm) ||
                task.Status.ToString().ToLower().Contains(lowerSearchTerm)
            ).ToList();
        }
    }
}