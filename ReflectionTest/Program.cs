using System;
using System.Reflection;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Load the assembly
            Assembly assembly = Assembly.LoadFrom(@"../TaskTracker/bin/Debug/net8.0/TaskTracker.dll");
            
            // Get the TaskManager type
            Type taskManagerType = assembly.GetType("TaskTracker.Services.TaskManager");
            
            if (taskManagerType == null)
            {
                Console.WriteLine("TaskManager type not found!");
                return;
            }
            
            Console.WriteLine($"TaskManager type found: {taskManagerType.FullName}");
            
            // Check if the methods exist
            MethodInfo[] methods = taskManagerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            
            string[] methodNames = { "LinearSearch", "LinqSort", "Sort" };
            
            foreach (string methodName in methodNames)
            {
                MethodInfo method = Array.Find(methods, m => m.Name == methodName);
                if (method != null)
                {
                    Console.WriteLine($"Method found: {method.Name}");
                }
                else
                {
                    Console.WriteLine($"Method NOT found: {methodName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}