using UnityEngine;
using TMPro;

public class CityStatistics : Singleton<CityStatistics>
{
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI populationText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI waterText;
    [SerializeField] private TextMeshProUGUI coalText;

    City city;

    protected override void Awake()
    {
        base.Awake();

        city = City.GetInstance();

        UpdateUI();
    }

    public void UpdateUI()
    {
        //Maybe we could have the UI read a value instead of updating it individually?

        //Update Toolbar
        dayText.text = $"Day: {city.Day}";

        //Update Panel
        populationText.text = $"Population: {city.Population}";
        moneyText.text = $"Money: ${city.Money}";
        powerText.text = $"Power: {city.Power}MW";
        waterText.text = $"Water: {city.Water}kL";
        coalText.text = $"Coal: {city.Coal} Ton";
    }
}