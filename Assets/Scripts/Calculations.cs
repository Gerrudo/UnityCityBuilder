public class Calculations
{
    public static int Population(int maxPopulation, int happiness, int currentPopulation)
    {
        bool isMaxPopulation = currentPopulation >= maxPopulation;

        if (!isMaxPopulation && happiness == 3)
        {
            currentPopulation += 10;
        }
        else if (!isMaxPopulation && happiness == 0)
        {
            currentPopulation -= 10;
        }

        return currentPopulation;
    }

    public static int Water(int currentPopulation)
    {
        int waterDemand = currentPopulation;

        return waterDemand;
    }

    public static int Power(int currentPopulation)
    {
        int powerDemand = currentPopulation;

        return powerDemand;
    }

    public static int Income(int currentPopulation)
    {
        int income = currentPopulation * 10;

        return income;
    }

    public static int Expenses(int currentPopulation)
    {
        int expenses = currentPopulation * 10;

        return expenses;
    }

    public static int Happiness()
    {
        int happiness = 3;

        return happiness;
    }
}