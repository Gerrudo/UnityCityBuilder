using System;
using System.Collections.Generic;

public interface IEmployer
{
    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
}
