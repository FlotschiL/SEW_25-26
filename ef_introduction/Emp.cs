using System;
using System.Collections.Generic;

namespace ef_introduction;

public partial class Emp
{
    public int Id { get; set; }

    public string Ename { get; set; } = null!;

    public string Job { get; set; } = null!;

    public int? ParentId { get; set; }

    public DateTime Hiredate { get; set; }

    public int Sal { get; set; }

    public int? Comm { get; set; }

    public int? DeptId { get; set; }

    public virtual Dept? Dept { get; set; }

    public virtual ICollection<Emp> InverseParent { get; set; } = new List<Emp>();

    public virtual Emp? Parent { get; set; }
    
    public override string ToString()
    {
        // Format the date for better readability (e.g., Jan 12, 2026)
        string dateStr = Hiredate.ToString("MMM dd, yyyy");
    
        // Handle the Manager/Parent name safely
        string managerInfo = Parent != null ? $" (Reports to: {Parent.Ename})" : " (No Manager)";

        return $"[ID: {Id}] {Ename,-10} | Job: {Job,-9} | Hired: {dateStr} | Sal: {Sal:C0}{managerInfo}";
    }
}
