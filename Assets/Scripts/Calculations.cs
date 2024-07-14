public static class Calculations
{
    public static int GetPopulation(int maxPopulation, int currentPopulation)
    {
        if (currentPopulation <= maxPopulation)
        {
            currentPopulation++;
        }

        return currentPopulation;
    }

    public static int ConsumeWater(int currentPopulation)
    {
        return currentPopulation * 2;
    }

    public static int ConsumePower(int currentPopulation)
    {
        return currentPopulation * 4;
    }

    public static int GeneratePower(int workers,  int maxWorkers)
    {
        var powerGenerated = 0;
       
        if (workers < maxWorkers)
        {
            powerGenerated = workers * 3;
        }

        return powerGenerated;
    }
    
    public static int GenerateWater()
    {
        return 1;
    }

    public static int PayTaxes(int currentPopulation)
    {
        return currentPopulation * 4;
    }
}