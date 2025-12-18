using TaskTracker.Models;
using TaskTracker.Services.Interfaces;

namespace TaskTracker.Services
{
    /// <summary>
    /// Service for generating reports based on tasks
    /// </summary>
    public class ReportGenerator
    {
        private readonly ITaskService _taskService;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the ReportGenerator class
        /// </summary>
        /// <param name="taskService">The task service to use for data</param>
        /// <param name="logger">The logger instance</param>
        public ReportGenerator(ITaskService taskService, ILogger logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        /// <summary>
        /// Generates a report of overdue tasks
        /// </summary>
        /// <returns>A formatted string containing the overdue tasks report</returns>
        public string GenerateOverdueTasksReport()
        {
            _logger.LogInfo("Generating overdue tasks report");
            
            if (_taskService is TaskManager taskManager)
            {
                var overdueTasks = taskManager.GetOverdueTasks();
                
                if (!overdueTasks.Any())
                {
                    return "No overdue tasks found.";
                }

                var report = new System.Text.StringBuilder();
                report.AppendLine("=== OVERDUE TASKS REPORT ===");
                report.AppendLine($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine($"Total overdue tasks: {overdueTasks.Count}");
                report.AppendLine();
                report.AppendLine("Overdue Tasks:");
                report.AppendLine("----------------------------------------");

                foreach (var task in overdueTasks)
                {
                    report.AppendLine($"ID: {task.Id}");
                    report.AppendLine($"Title: {task.Title}");
                    report.AppendLine($"Assignee: {task.Assignee}");
                    report.AppendLine($"Due Date: {task.DueDate:yyyy-MM-dd}");
                    report.AppendLine($"Days Overdue: {(DateTime.Now - task.DueDate).Days}");
                    report.AppendLine("----------------------------------------");
                }

                return report.ToString();
            }

            return "Unable to generate report: Task service is not compatible.";
        }

        /// <summary>
        /// Generates a report of tasks by assignee
        /// </summary>
        /// <returns>A formatted string containing the tasks by assignee report</returns>
        public string GenerateTasksByAssigneeReport()
        {
            _logger.LogInfo("Generating tasks by assignee report");
            
            if (_taskService is TaskManager taskManager)
            {
                var tasksByAssignee = taskManager.GetTasksByAssignee();
                
                if (!tasksByAssignee.Any())
                {
                    return "No tasks found.";
                }

                var report = new System.Text.StringBuilder();
                report.AppendLine("=== TASKS BY ASSIGNEE REPORT ===");
                report.AppendLine($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine();
                
                foreach (var kvp in tasksByAssignee)
                {
                    report.AppendLine($"Assignee: {kvp.Key}");
                    report.AppendLine($"Task Count: {kvp.Value.Count}");
                    report.AppendLine("Tasks:");
                    
                    foreach (var task in kvp.Value)
                    {
                        report.AppendLine($"  - ID: {task.Id}, Title: {task.Title}, Status: {task.Status}, Due: {task.DueDate:yyyy-MM-dd}");
                    }
                    
                    report.AppendLine();
                }

                return report.ToString();
            }

            return "Unable to generate report: Task service is not compatible.";
        }

        /// <summary>
        /// Generates a report of upcoming deadlines
        /// </summary>
        /// <param name="days">Number of days to look ahead</param>
        /// <returns>A formatted string containing the upcoming deadlines report</returns>
        public string GenerateUpcomingDeadlinesReport(int days = 7)
        {
            _logger.LogInfo($"Generating upcoming deadlines report for next {days} days");
            
            if (_taskService is TaskManager taskManager)
            {
                var upcomingTasks = taskManager.GetUpcomingDeadlines(days);
                
                if (!upcomingTasks.Any())
                {
                    return $"No upcoming deadlines found within the next {days} days.";
                }

                var report = new System.Text.StringBuilder();
                report.AppendLine("=== UPCOMING DEADLINES REPORT ===");
                report.AppendLine($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine($"Next {days} days");
                report.AppendLine($"Total upcoming tasks: {upcomingTasks.Count}");
                report.AppendLine();
                report.AppendLine("Upcoming Tasks:");
                report.AppendLine("----------------------------------------");

                foreach (var task in upcomingTasks.OrderBy(t => t.DueDate))
                {
                    report.AppendLine($"ID: {task.Id}");
                    report.AppendLine($"Title: {task.Title}");
                    report.AppendLine($"Assignee: {task.Assignee}");
                    report.AppendLine($"Due Date: {task.DueDate:yyyy-MM-dd}");
                    report.AppendLine($"Days Until Due: {(task.DueDate - DateTime.Now).Days}");
                    report.AppendLine("----------------------------------------");
                }

                return report.ToString();
            }

            return "Unable to generate report: Task service is not compatible.";
        }
    }
}