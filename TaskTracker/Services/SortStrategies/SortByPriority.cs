using System;
using System.Collections.Generic;
using TaskTracker.Models;
using TaskTracker.Services.Interfaces;

namespace TaskTracker.Services.SortStrategies
{
    /// <summary>
    /// Sorting strategy that sorts tasks by priority
    /// </summary>
    public class SortByPriority : ISortStrategy
    {
        /// <summary>
        /// Sorts tasks by priority using bubble sort algorithm
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
                        GetPriorityValue(sortedTasks[j].Priority) > GetPriorityValue(sortedTasks[j + 1].Priority) : 
                        GetPriorityValue(sortedTasks[j].Priority) < GetPriorityValue(sortedTasks[j + 1].Priority);
                    
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
        
        /// <summary>
        /// Converts priority enum to numeric value for comparison
        /// </summary>
        /// <param name="priority">The priority to convert</param>
        /// <returns>Numeric value representing the priority</returns>
        private int GetPriorityValue(Priority priority)
        {
            return priority switch
            {
                Priority.Low => 1,
                Priority.Medium => 2,
                Priority.High => 3,
                _ => 0
            };
        }
    }
}