using System;
using System.Collections.Generic;
using System.Linq;
using TaskTracker.Models;
using TaskTracker.Services.Interfaces;
using TaskTracker.Data.Repositories;
using TaskTracker.Services.SortStrategies;
using TaskTracker.Data.Exceptions;

namespace TaskTracker.Services
{
    /// <summary>
    /// Service for managing tasks with business logic
    /// </summary>
    public class TaskManager : ITaskService
    {
        private readonly TaskRepository _repository;
        private readonly ILogger _logger;
        private readonly Dictionary<string, ISortStrategy> _sortStrategies;

        /// <summary>
        /// Initializes a new instance of the TaskManager class
        /// </summary>
        /// <param name="repository">The task repository</param>
        /// <param name="logger">The logger instance</param>
        public TaskManager(TaskRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
            
            // Initialize sorting strategies
            _sortStrategies = new Dictionary<string, ISortStrategy>
            {
                { "duedate", new SortByDueDate() },
                { "priority", new SortByPriority() },
                { "assignee", new SortByAssignee() }
            };
            
            _logger.LogInfo("TaskManager initialized with sorting strategies");
        }

        /// <summary>
        /// Gets all tasks
        /// </summary>
        /// <returns>A list of all tasks</returns>
        public List<Task> GetAll()
        {
            _logger.LogInfo("Retrieving all tasks");
            var tasks = _repository.GetAll();
            return tasks.ToList();
        }

        /// <summary>
        /// Gets a task by its ID
        /// </summary>
        /// <param name="id">The ID of the task to retrieve</param>
        /// <returns>The task with the specified ID</returns>
        public Task? GetById(int id)
        {
            try
            {
                _logger.LogInfo($"Retrieving task with ID {id}");
                return _repository.GetById(id);
            }
            catch (TaskNotFoundException)
            {
                _logger.LogWarning($"Task with ID {id} not found");
                return null;
            }
        }

        /// <summary>
        /// Adds a new task
        /// </summary>
        /// <param name="task">The task to add</param>
        public void Add(Task task)
        {
            try
            {
                Utilities.Validator.ValidateTask(task);
                _repository.Add(task);
                _logger.LogInfo($"Task '{task.Title}' added successfully with ID {task.Id}");
            }
            catch (InvalidTaskDataException ex)
            {
                _logger.LogError($"Failed to add task: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing task
        /// </summary>
        /// <param name="task">The task with updated information</param>
        public void Update(Task task)
        {
            try
            {
                Utilities.Validator.ValidateTask(task);
                _repository.Update(task);
                _logger.LogInfo($"Task '{task.Title}' updated successfully");
            }
            catch (InvalidTaskDataException ex)
            {
                _logger.LogError($"Failed to update task: {ex.Message}");
                throw;
            }
            catch (TaskNotFoundException ex)
            {
                _logger.LogError($"Failed to update task: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes a task by its ID
        /// </summary>
        /// <param name="id">The ID of the task to delete</param>
        public void Delete(int id)
        {
            try
            {
                Utilities.Validator.ValidateId(id);
                _repository.Delete(id);
                _logger.LogInfo($"Task with ID {id} deleted successfully");
            }
            catch (InvalidTaskDataException ex)
            {
                _logger.LogError($"Failed to delete task: {ex.Message}");
                throw;
            }
            catch (TaskNotFoundException ex)
            {
                _logger.LogError($"Failed to delete task: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Searches for tasks based on a search term using LINQ
        /// </summary>
        /// <param name="searchTerm">The term to search for in task properties</param>
        /// <returns>A list of tasks matching the search criteria</returns>
        public List<Task> Search(string searchTerm)
        {
            _logger.LogInfo($"Searching tasks with term: '{searchTerm}'");
            var tasks = _repository.Search(searchTerm);
            return tasks.ToList();
        }

        /// <summary>
        /// Searches for tasks based on a search term using linear search algorithm
        /// </summary>
        /// <param name="searchTerm">The term to search for in task properties</param>
        /// <returns>A list of tasks matching the search criteria</returns>
        public List<Task> LinearSearch(string searchTerm)
        {
            _logger.LogInfo($"Linear searching tasks with term: '{searchTerm}'");
            var allTasks = _repository.GetAll();
            var results = new List<Task>();
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return allTasks.ToList();
            }
            
            var lowerSearchTerm = searchTerm.ToLower();
            
            // Linear search implementation
            foreach (var task in allTasks)
            {
                if (task.Id.ToString().Contains(searchTerm) ||
                    task.Title.ToLower().Contains(lowerSearchTerm) ||
                    task.Assignee.ToLower().Contains(lowerSearchTerm) ||
                    task.Status.ToString().ToLower().Contains(lowerSearchTerm))
                {
                    results.Add(task);
                }
            }
            
            return results;
        }

        /// <summary>
        /// Sorts tasks using a specified sorting strategy
        /// </summary>
        /// <param name="strategyName">The name of the sorting strategy to use</param>
        /// <param name="ascending">Whether to sort in ascending order</param>
        /// <returns>A sorted list of tasks</returns>
        public List<Task> Sort(string strategyName, bool ascending = true)
        {
            var tasks = GetAll();
            
            if (!_sortStrategies.ContainsKey(strategyName.ToLower()))
            {
                _logger.LogWarning($"Sorting strategy '{strategyName}' not found. Returning unsorted tasks.");
                return tasks.ToList();
            }
            
            _logger.LogInfo($"Sorting {tasks.Count} tasks by {strategyName} in {(ascending ? "ascending" : "descending")} order");
            var sortedTasks = _sortStrategies[strategyName.ToLower()].Sort(tasks, ascending);
            return sortedTasks.ToList();
        }

        /// <summary>
        /// Sorts tasks using built-in LINQ sorting for comparison
        /// </summary>
        /// <param name="sortBy">The field to sort by (duedate, priority, assignee, createddate)</param>
        /// <param name="ascending">Whether to sort in ascending order</param>
        /// <returns>A sorted list of tasks</returns>
        public List<Task> LinqSort(string sortBy, bool ascending = true)
        {
            var tasks = GetAll();
            _logger.LogInfo($"LINQ sorting {tasks.Count} tasks by {sortBy} in {(ascending ? "ascending" : "descending")} order");
            
            var result = sortBy.ToLower() switch
            {
                "duedate" => ascending 
                    ? tasks.OrderBy(t => t.DueDate).ToList()
                    : tasks.OrderByDescending(t => t.DueDate).ToList(),
                "priority" => ascending
                    ? tasks.OrderBy(t => t.Priority).ToList()
                    : tasks.OrderByDescending(t => t.Priority).ToList(),
                "assignee" => ascending
                    ? tasks.OrderBy(t => t.Assignee).ToList()
                    : tasks.OrderByDescending(t => t.Assignee).ToList(),
                "createddate" => ascending
                    ? tasks.OrderBy(t => t.CreatedDate).ToList()
                    : tasks.OrderByDescending(t => t.CreatedDate).ToList(),
                _ => tasks
            };
            
            return result.ToList();
        }

        /// <summary>
        /// Gets all overdue tasks
        /// </summary>
        /// <returns>A list of overdue tasks</returns>
        public List<Task> GetOverdueTasks()
        {
            _logger.LogInfo("Retrieving overdue tasks");
            var tasks = _repository.GetAll().Where(t => t.IsOverdue()).ToList();
            return tasks.ToList();
        }

        /// <summary>
        /// Gets tasks grouped by assignee
        /// </summary>
        /// <returns>A dictionary with assignees as keys and their tasks as values</returns>
        public Dictionary<string, List<Task>> GetTasksByAssignee()
        {
            _logger.LogInfo("Retrieving tasks grouped by assignee");
            var tasks = _repository.GetAll();
            return tasks.GroupBy(t => t.Assignee).ToDictionary(
                g => g.Key, 
                g => g.ToList());
        }

        /// <summary>
        /// Gets upcoming deadline tasks
        /// </summary>
        /// <param name="days">Number of days to look ahead</param>
        /// <returns>A list of tasks with upcoming deadlines</returns>
        public List<Task> GetUpcomingDeadlines(int days = 7)
        {
            _logger.LogInfo($"Retrieving tasks with deadlines in the next {days} days");
            var cutoffDate = DateTime.Now.AddDays(days);
            var tasks = _repository.GetAll();
            var filteredTasks = tasks
                .Where(t => t.DueDate >= DateTime.Now && t.DueDate <= cutoffDate && t.Status != Status.Done)
                .ToList();
            return filteredTasks;
        }
    }
}