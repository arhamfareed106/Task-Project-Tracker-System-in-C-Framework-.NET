using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TaskTracker.App
{
    // Simple Task class
public class Task
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; }
    public Status Status { get; set; }
    public string Assignee { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? CompletedDate { get; set; }

    public Task()
    {
        // Used by JSON serializer
    }

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

    public bool IsOverdue()
    {
        return Status != Status.Done && DueDate < DateTime.Now;
    }

    public override string ToString()
    {
        return $"ID: {Id} | Title: {Title} | Due: {DueDate:yyyy-MM-dd} | Priority: {Priority} | Status: {Status} | Assignee: {Assignee}";
    }
}

// Enums
public enum Priority
{
    Low = 0,
    Medium = 1,
    High = 2
}

public enum Status
{
    ToDo = 0,
    InProgress = 1,
    Done = 2
}

// Simple file handler
public class FileHandler
{
    private readonly string _filePath = "tasks.json";

    public List<Task> ReadTasks()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine($"Tasks file not found at {_filePath}. Creating new file.");
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
            Console.WriteLine($"Successfully read {tasks.Count} tasks from {_filePath}");
            return tasks;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading tasks from {_filePath}: {ex.Message}");
            return new List<Task>();
        }
    }

    public void WriteTasks(List<Task> tasks)
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
            Console.WriteLine($"Successfully wrote {tasks.Count} tasks to {_filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing tasks to {_filePath}: {ex.Message}");
        }
    }
}

// Simple task repository
public class TaskRepository
{
    private List<Task> _tasks;
    private readonly FileHandler _fileHandler;

    public TaskRepository()
    {
        _fileHandler = new FileHandler();
        _tasks = new List<Task>();
        LoadTasks();
    }

    private void LoadTasks()
    {
        try
        {
            _tasks = _fileHandler.ReadTasks();
            Console.WriteLine($"Loaded {_tasks.Count} tasks from repository");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load tasks: {ex.Message}");
            _tasks = new List<Task>(); // Initialize with empty list as fallback
        }
    }

    private void SaveTasks()
    {
        try
        {
            _fileHandler.WriteTasks(_tasks);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save tasks: {ex.Message}");
        }
    }

    public List<Task> GetAll()
    {
        return new List<Task>(_tasks);
    }

    public Task? GetById(int id)
    {
        return _tasks.FirstOrDefault(t => t.Id == id);
    }

    public void Add(Task task)
    {
        _tasks.Add(task);
        SaveTasks();
        Console.WriteLine($"Added task with ID {task.Id}");
    }

    public void Update(Task task)
    {
        var index = _tasks.FindIndex(t => t.Id == task.Id);
        if (index != -1)
        {
            _tasks[index] = task;
            SaveTasks();
            Console.WriteLine($"Updated task with ID {task.Id}");
        }
    }

    public void Delete(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task != null)
        {
            _tasks.Remove(task);
            SaveTasks();
            Console.WriteLine($"Deleted task with ID {id}");
        }
    }

    public List<Task> Search(string searchTerm)
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

// Simple task manager
public class TaskManager
{
    private readonly TaskRepository _repository;

    public TaskManager()
    {
        _repository = new TaskRepository();
    }

    public List<Task> GetAll()
    {
        return _repository.GetAll();
    }

    public Task? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public void Add(Task task)
    {
        _repository.Add(task);
    }

    public void Update(Task task)
    {
        _repository.Update(task);
    }

    public void Delete(int id)
    {
        _repository.Delete(id);
    }

    public List<Task> Search(string searchTerm)
    {
        return _repository.Search(searchTerm);
    }

    public int GenerateUniqueId()
    {
        var tasks = _repository.GetAll();
        if (!tasks.Any())
        {
            return 1;
        }

        return tasks.Max(t => t.Id) + 1;
    }
}

// Main program
public class Program
{
    private static TaskManager? _taskManager;

    public static void Main(string[] args)
    {
        try
        {
            InitializeServices();
            DisplayWelcomeMessage();
            RunMainMenu();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    private static void InitializeServices()
    {
        _taskManager = new TaskManager();
        Console.WriteLine("Application services initialized successfully");
    }

    private static void DisplayWelcomeMessage()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("       SIMPLE TASK TRACKER");
        Console.WriteLine("========================================");
        Console.WriteLine();
    }

    private static void RunMainMenu()
    {
        while (true)
        {
            DisplayMainMenu();
            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    CreateTask();
                    break;
                case "2":
                    ViewAllTasks();
                    break;
                case "3":
                    UpdateTask();
                    break;
                case "4":
                    DeleteTask();
                    break;
                case "5":
                    SearchTasks();
                    break;
                case "6":
                    Console.WriteLine("Thank you for using Task Tracker!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void DisplayMainMenu()
    {
        Console.Clear();
        DisplayWelcomeMessage();
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("---------");
        Console.WriteLine("1. Create Task");
        Console.WriteLine("2. View All Tasks");
        Console.WriteLine("3. Update Task");
        Console.WriteLine("4. Delete Task");
        Console.WriteLine("5. Search Tasks");
        Console.WriteLine("6. Exit");
        Console.WriteLine();
        Console.Write("Please select an option (1-6): ");
    }

    private static void CreateTask()
    {
        Console.Clear();
        Console.WriteLine("CREATE NEW TASK");
        Console.WriteLine("----------------");

        try
        {
            // Get task details from user
            Console.Write("Enter task title: ");
            var title = Console.ReadLine()?.Trim();

            Console.Write("Enter task description: ");
            var description = Console.ReadLine()?.Trim();

            Console.Write("Enter assignee name: ");
            var assignee = Console.ReadLine()?.Trim();

            Console.Write("Enter due date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine()?.Trim(), out var dueDate))
            {
                Console.WriteLine("Invalid date format. Using today's date.");
                dueDate = DateTime.Today;
            }

            Console.WriteLine("Select priority:");
            Console.WriteLine("1. Low");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. High");
            Console.Write("Enter choice (1-3): ");
            var priorityChoice = Console.ReadLine()?.Trim();
            var priority = priorityChoice switch
            {
                "1" => Priority.Low,
                "2" => Priority.Medium,
                "3" => Priority.High,
                _ => Priority.Medium
            };

            // Generate a unique ID
            var id = _taskManager?.GenerateUniqueId() ?? 1;

            // Create and add the task
            var task = new Task(id, title ?? "", description ?? "", dueDate, priority, assignee ?? "");
            _taskManager?.Add(task);

            Console.WriteLine($"Task '{title}' created successfully with ID {id}!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void ViewAllTasks()
    {
        Console.Clear();
        Console.WriteLine("ALL TASKS");
        Console.WriteLine("----------");

        try
        {
            var tasks = _taskManager?.GetAll();

            if (tasks == null || !tasks.Any())
            {
                Console.WriteLine("No tasks found.");
            }
            else
            {
                DisplayTasks(tasks);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving tasks: {ex.Message}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void UpdateTask()
    {
        Console.Clear();
        Console.WriteLine("UPDATE TASK");
        Console.WriteLine("------------");

        try
        {
            Console.Write("Enter task ID to update: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out var id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var task = _taskManager?.GetById(id);
            if (task == null)
            {
                Console.WriteLine($"Task with ID {id} not found.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            // Display current task details
            Console.WriteLine($"Current details for task '{task.Title}':");
            Console.WriteLine($"1. Title: {task.Title}");
            Console.WriteLine($"2. Description: {task.Description}");
            Console.WriteLine($"3. Assignee: {task.Assignee}");
            Console.WriteLine($"4. Due Date: {task.DueDate:yyyy-MM-dd}");
            Console.WriteLine($"5. Priority: {task.Priority}");
            Console.WriteLine($"6. Status: {task.Status}");
            Console.WriteLine();

            // Get updated details
            Console.Write("Enter new title (or press Enter to keep current): ");
            var title = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(title))
            {
                task.Title = title;
            }

            Console.Write("Enter new description (or press Enter to keep current): ");
            var description = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(description))
            {
                task.Description = description;
            }

            Console.Write("Enter new assignee (or press Enter to keep current): ");
            var assignee = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(assignee))
            {
                task.Assignee = assignee;
            }

            Console.Write("Enter new due date (yyyy-mm-dd) (or press Enter to keep current): ");
            var dueDateInput = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(dueDateInput))
            {
                if (DateTime.TryParse(dueDateInput, out var dueDate))
                {
                    task.DueDate = dueDate;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Keeping current date.");
                }
            }

            Console.WriteLine("Select new priority (or press Enter to keep current):");
            Console.WriteLine("1. Low");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. High");
            Console.Write("Enter choice (1-3): ");
            var priorityChoice = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(priorityChoice))
            {
                var priority = priorityChoice switch
                {
                    "1" => Priority.Low,
                    "2" => Priority.Medium,
                    "3" => Priority.High,
                    _ => task.Priority
                };
                task.Priority = priority;
            }

            Console.WriteLine("Select new status (or press Enter to keep current):");
            Console.WriteLine("1. ToDo");
            Console.WriteLine("2. InProgress");
            Console.WriteLine("3. Done");
            Console.Write("Enter choice (1-3): ");
            var statusChoice = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(statusChoice))
            {
                var status = statusChoice switch
                {
                    "1" => Status.ToDo,
                    "2" => Status.InProgress,
                    "3" => Status.Done,
                    _ => task.Status
                };
                task.UpdateStatus(status);
            }

            _taskManager?.Update(task);
            Console.WriteLine($"Task '{task.Title}' updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void DeleteTask()
    {
        Console.Clear();
        Console.WriteLine("DELETE TASK");
        Console.WriteLine("------------");

        try
        {
            Console.Write("Enter task ID to delete: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out var id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var task = _taskManager?.GetById(id);
            if (task == null)
            {
                Console.WriteLine($"Task with ID {id} not found.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Are you sure you want to delete task '{task.Title}'? (y/N)");
            var confirmation = Console.ReadLine()?.Trim().ToLower();

            if (confirmation == "y" || confirmation == "yes")
            {
                _taskManager?.Delete(id);
                Console.WriteLine($"Task '{task.Title}' deleted successfully!");
            }
            else
            {
                Console.WriteLine("Task deletion cancelled.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void SearchTasks()
    {
        Console.Clear();
        Console.WriteLine("SEARCH TASKS");
        Console.WriteLine("------------");

        try
        {
            Console.Write("Enter search term (ID, Title, Assignee, or Status): ");
            var searchTerm = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine("Search term cannot be empty.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            var tasks = _taskManager?.Search(searchTerm);

            if (tasks == null || !tasks.Any())
            {
                Console.WriteLine($"No tasks found matching '{searchTerm}'.");
            }
            else
            {
                Console.WriteLine($"Found {tasks.Count} task(s) matching '{searchTerm}':\n");
                DisplayTasks(tasks);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching tasks: {ex.Message}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void DisplayTasks(List<Task> tasks)
    {
        foreach (var task in tasks)
        {
            // Color coding for different statuses and priorities
            var statusColor = task.Status switch
            {
                Status.ToDo => ConsoleColor.Blue,
                Status.InProgress => ConsoleColor.Yellow,
                Status.Done => ConsoleColor.Green,
                _ => Console.ForegroundColor
            };

            var priorityColor = task.Priority switch
            {
                Priority.High => ConsoleColor.Red,
                Priority.Medium => ConsoleColor.Yellow,
                Priority.Low => ConsoleColor.Green,
                _ => Console.ForegroundColor
            };

            Console.ForegroundColor = statusColor;
            Console.Write($"[{task.Status}] ");
            Console.ForegroundColor = priorityColor;
            Console.Write($"[{task.Priority}] ");
            Console.ResetColor();
            Console.WriteLine($"{task.Id}: {task.Title} (Due: {task.DueDate:yyyy-MM-dd}, Assignee: {task.Assignee})");
        }
        Console.WriteLine();
    }
}

}