public class Calculations
{
    public static double GetPercentage(double part, double whole) => (part / whole) * 100;

    public static float Normalise(float total, int count) => total / count;
}