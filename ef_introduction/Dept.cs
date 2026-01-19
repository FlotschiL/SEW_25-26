using System;
using System.Collections.Generic;

namespace ef_introduction;

public partial class Dept
{
    public int Deptno { get; set; }

    public string Dname { get; set; } = null!;

    public string Loc { get; set; } = null!;

    public virtual ICollection<Emp> Emps { get; set; } = new List<Emp>();

    public override string ToString()
    {
        return $"[Dept {Deptno}] Name: {Dname} | Location: {Loc} | Employees: {Emps?.Count ?? 0}";
    }
}
