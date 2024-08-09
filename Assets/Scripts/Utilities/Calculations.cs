using System;

public class Calculations
{
    public static double GetPercentage(double part, double whole) => (part / whole) * 100;

    public static float Normalise(float total, int count) => (float)Math.Round(((double)total / count), 1);
}