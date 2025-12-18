using TaskTracker.Models;
using TaskTracker.Services.Interfaces;

namespace TaskTracker.Services.SortStrategies
{
    /// <summary>
    /// Sorting strategy that sorts tasks by due date
    /// </summary>
    public class SortByDueDate : ISortStrategy
    {
        /// <summary>
        /// Sorts tasks by due date using bubble sort algorithm
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
                        sortedTasks[j].DueDate > sortedTasks[j + 1].DueDate : 
                        sortedTasks[j].DueDate < sortedTasks[j + 1].DueDate;
                    
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