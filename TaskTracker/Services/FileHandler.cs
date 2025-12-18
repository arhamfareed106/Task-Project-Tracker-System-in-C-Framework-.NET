using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TaskTracker.Models;
using TaskTracker.Data.Exceptions;
using TaskTracker.Services.Interfaces;

namespace TaskTracker.Services
{
    /// <summary>
    /// Service for handling file operations such as reading and writing tasks to JSON
    /// </summary>
    public class FileHandler
    {
        private readonly string _filePath;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the FileHandler class
        /// </summary>
        /// <param name="filePath">The path to the JSON file</param>
        /// <param name="logger">The logger instance</param>
        public FileHandler(string filePath, ILogger logger)
        {
            _filePath = filePath;
            _logger = logger;
        }

        /// <summary>
        /// Reads tasks from the JSON file
        /// </summary>
        /// <returns>A list of tasks</returns>
        public virtual List<Task> ReadTasks()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    _logger.LogInfo($"Tasks file not found at {_filePath}. Creating new file.");
                    WriteTasks(new List<Task>());
                    return new List<Task>();
                }

                var json = File.ReadAllText(_filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                var tasks = JsonSerializer.Deserialize<List<Task>>(json, options) ?? new List<Task>();
                _logger.LogInfo($"Successfully read {tasks.Count} tasks from {_filePath}");
                return tasks;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Invalid JSON format in {_filePath}: {ex.Message}");
                throw new InvalidTaskDataException($"Invalid JSON format in {_filePath}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error reading tasks from {_filePath}: {ex.Message}");
                throw new InvalidOperationException($"Error reading tasks from {_filePath}", ex);
            }
        }

        /// <summary>
        /// Writes tasks to the JSON file
        /// </summary>
        /// <param name="tasks">The list of tasks to write</param>
        public virtual void WriteTasks(List<Task> tasks)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(tasks, options);
                File.WriteAllText(_filePath, json);
                _logger.LogInfo($"Successfully wrote {tasks.Count} tasks to {_filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error writing tasks to {_filePath}: {ex.Message}");
                throw new InvalidOperationException($"Error writing tasks to {_filePath}", ex);
            }
        }
    }
}