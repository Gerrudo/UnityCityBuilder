using System.Collections.Generic;

public interface IResidence
{
    public int MaxPopulation { get; set; }
    public List<Citizen> Residents { get; set; } 
}