# Task & Project Tracker System

This is a university assignment project implementing a console-based Task & Project Tracker in C#.



## Features

- Create, read, update, and delete tasks
- Search tasks using both LINQ and linear search algorithms
- Sort tasks using both bubble sort (manual implementation) and LINQ sorting
- Generate reports (overdue tasks, tasks by assignee, upcoming deadlines)
- Data persistence using JSON files
- Activity logging to text files
- Implementation of design patterns:
  - Singleton (Logger)
  - Repository (TaskRepository)
  - Strategy (SortStrategies)
- Unit testing with xUnit
- Professional error handling with custom exceptions
- Clean, modular architecture

## Technical Specifications

- Built with .NET 8.0
- Console application
- Layered architecture (Models, Services, Data, Utilities)
- Manual implementation of sorting algorithms (Bubble Sort)
- Linear search implementation
- Comparison with built-in LINQ search and sorting
- XML documentation comments on all public methods

## Project Structure

```
TaskTracker/
├── Program.cs (main menu logic)
├── Models/
│   ├── Task.cs (with IComparable<Task>)
│   ├── Priority.cs (enum)
│   └── Status.cs (enum)
├── Services/
│   ├── Interfaces/
│   │   ├── ITaskService.cs
│   │   ├── ILogger.cs
│   │   └── ISortStrategy.cs
│   ├── TaskManager.cs (implements ITaskService)
│   ├── Logger.cs (Singleton)
│   ├── FileHandler.cs
│   ├── ReportGenerator.cs
│   └── SortStrategies/
│       ├── SortByDueDate.cs
│       ├── SortByPriority.cs
│       └── SortByAssignee.cs
├── Data/
│   ├── repositories/
│   │   └── TaskRepository.cs
│   └── exceptions/
│       ├── TaskNotFoundException.cs
│       └── InvalidTaskDataException.cs
├── Utilities/
│   ├── Validator.cs
│   └── ExtensionMethods.cs
├── Tests/
│   └── TaskTracker.Tests/
│       ├── TaskManagerTests.cs
│       ├── FileHandlerTests.cs
│       └── SortStrategyTests.cs
└── DataFiles/
    ├── tasks.json
    └── activity_log.txt
```

## Prerequisites

- .NET 8.0 SDK

## How to Build and Run

1. Open a terminal/command prompt
2. Navigate to the TaskTracker project directory
3. Run the following commands:

```bash
dotnet restore
dotnet build
dotnet run
```

## How to Run Tests

1. Navigate to the TaskTracker.Tests directory
2. Run the following commands:

```bash
dotnet test
```

## Usage

When you run the application, you'll see a menu with the following options:

1. Create Task - Add a new task with title, description, due date, priority, and assignee
2. View All Tasks - Display all tasks with color-coded status and priority
3. Update Task - Modify an existing task's details
4. Delete Task - Remove a task by ID
5. Search Tasks (LINQ) - Find tasks by ID, title, assignee, or status using LINQ
6. Search Tasks (Linear) - Find tasks by ID, title, assignee, or status using linear search
7. Sort Tasks (Bubble) - Sort tasks by due date, priority, or assignee using bubble sort (ascending or descending)
8. Sort Tasks (LINQ) - Sort tasks by due date, priority, assignee, or created date using LINQ (ascending or descending)
9. Generate Reports - Create reports for overdue tasks, tasks by assignee, or upcoming deadlines
10. Exit - Close the application

## Design Decisions

1. **Singleton Pattern for Logger**: Ensures only one instance of the logger exists throughout the application lifecycle, providing centralized logging.

2. **Repository Pattern for Task Management**: Separates data access logic from business logic, making the code more maintainable and testable.

3. **Strategy Pattern for Sorting**: Allows easy addition of new sorting algorithms without modifying existing code, following the Open/Closed Principle.

4. **Manual Sorting Implementation**: Implemented bubble sort to demonstrate understanding of algorithms, while also providing built-in LINQ sorting for comparison.

5. **Linear Search Implementation**: Implemented linear search to demonstrate understanding of search algorithms, while also providing built-in LINQ search for comparison.

6. **Custom Exceptions**: Created specific exception types for better error handling and clearer error messages.

7. **JSON Serialization**: Used System.Text.Json for data persistence with pretty printing for human-readable files.

8. **Thread-Safe Logging**: Implemented thread-safe logging using locks to prevent concurrency issues.

9. **Input Validation**: Comprehensive validation for task data to ensure data integrity.

## Error Handling

The application handles various error scenarios:
- Invalid JSON format
- Missing data files
- Duplicate task IDs
- Invalid dates
- Null or empty input strings
- File permission issues

All errors are logged appropriately and user-friendly messages are displayed.

## Advanced Features

- Task categories/tags (planned for future implementation)
- Task dependencies (planned for future implementation)
- Recurring tasks (planned for future implementation)
- Export to CSV functionality (planned for future implementation)
- Statistics dashboard (planned for future implementation)
- Undo/redo functionality (planned for future implementation)