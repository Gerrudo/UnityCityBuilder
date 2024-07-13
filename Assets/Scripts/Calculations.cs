public static class Calculations
{
    public static int Population(int maxPopulation, int currentPopulation)
    {
        if (currentPopulation < maxPopulation)
        {
            currentPopulation++;
        }

        return currentPopulation;
    }

    public static int Water(int currentPopulation)
    {
        return currentPopulation;
    }

    public static int Power(int currentPopulation)
    {
        return currentPopulation;
    }

    public static int Income(int currentPopulation)
    {
        return currentPopulation * 10;
    }
}