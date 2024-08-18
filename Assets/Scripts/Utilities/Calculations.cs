using System;

public class Calculations
{
    public static double GetPercentage(double part, double whole) => (part / whole) * 100;

    public static double Normalise(double total, int count) => Math.Round(total / count, 1);
}