using UnityEngine;
using TMPro;

public class CityStatistics : Singleton<CityStatistics>
{
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI populationText;
    [SerializeField] private TextMeshProUGUI fundsText;
    [SerializeField] private TextMeshProUGUI earningsText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI waterText;
    [SerializeField] private TextMeshProUGUI goodsText;
    [SerializeField] private TextMeshProUGUI approvalText;
    [SerializeField] private TextMeshProUGUI unemployedText;
    
    private City city;
    
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
        dayText.text = $"{city.CityData.Day}";
        populationText.text = $"{city.CityData.Population}";
        fundsText.text = $"${city.CityData.Funds}";
        earningsText.text = city.CityData.Earnings > 0 ? $"${city.CityData.Earnings}+" : $"${city.CityData.Earnings}";

        //Update Panel
        powerText.text = $"Power: {city.CityData.Power}kW";
        waterText.text = $"Water: {city.CityData.Water}kL";
        goodsText.text = $"Goods: {city.CityData.Goods} Ton";
        approvalText.text = $"Approval: {city.CityData.ApprovalRating}%";
        unemployedText.text = $"Unemployed: {city.CityData.Unemployed}";
    }
}