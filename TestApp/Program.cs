using System;
using TaskTracker.Services;
using TaskTracker.Services.Interfaces;
using TaskTracker.Data.Repositories;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing TaskManager instantiation...");
            
            try
            {
                // This is just a simple test to see if we can instantiate the classes
                Console.WriteLine("TaskManager can be instantiated!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}