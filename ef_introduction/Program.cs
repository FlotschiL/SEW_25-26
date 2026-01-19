// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;

namespace ef_introduction;

public class Program
{
    public static void Main(string[] args)
    {
        var context = new ScottnewContext();
// Get a single employee by name
        var emp = context.Emps.FirstOrDefault(e => e.Ename == "KING");

        if (emp != null)
        {
            Console.WriteLine(emp.ToString());
        }
        
        var employeesWithDept = context.Emps
            .Include(e => e.Dept)
            .ToList();
        foreach (var e in employeesWithDept)
        {
            Console.WriteLine(e.ToString());
        }
        Console.WriteLine("Hello, World!");
    }
}
