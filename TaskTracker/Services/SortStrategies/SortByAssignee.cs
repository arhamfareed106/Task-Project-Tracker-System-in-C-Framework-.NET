using TaskTracker.Models;
using TaskTracker.Services.Interfaces;

namespace TaskTracker.Services.SortStrategies
{
    /// <summary>
    /// Sorting strategy that sorts tasks by assignee name
    /// </summary>
    public class SortByAssignee : ISortStrategy
    {
        /// <summary>
        /// Sorts tasks by assignee name using bubble sort algorithm
        /// </summary>
        /// <param name="tasks">The list of tasks to sort</param>
        /// <param name="ascending">Whether to sort in ascending order</param>
        /// <returns>A sorted list of tasks</returns>
        public List<Task> Sort(List<Task> tasks, bool ascending = true)
        {
            var sortedTasks = new List<Task>(tasks);
            
            // Bubble sort implementation
            for (int i = 0; i < sortedTasks.Count - 1; i++)
            {
                for (int j = 0; j < sortedTasks.Count - i - 1; j++)
                {
                    bool shouldSwap = ascending ? 
                        string.Compare(sortedTasks[j].Assignee, sortedTasks[j + 1].Assignee, StringComparison.OrdinalIgnoreCase) > 0 : 
                        string.Compare(sortedTasks[j].Assignee, sortedTasks[j + 1].Assignee, StringComparison.OrdinalIgnoreCase) < 0;
                    
                    if (shouldSwap)
                    {
                        var temp = sortedTasks[j];
                        sortedTasks[j] = sortedTasks[j + 1];
                        sortedTasks[j + 1] = temp;
                    }
                }
            }
            
            return sortedTasks;
        }
    }
}