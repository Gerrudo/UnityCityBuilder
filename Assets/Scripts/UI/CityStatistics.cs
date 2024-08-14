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
        dayText.text = $"{city.Day}";
        populationText.text = $"{city.Population}";
        fundsText.text = $"${city.Funds}";
        earningsText.text = city.Earnings > 0 ? $"${city.Earnings}+" : $"${city.Earnings}";

        //Update Panel
        powerText.text = $"Power: {city.Power}kW";
        waterText.text = $"Water: {city.Water}kL";
        goodsText.text = $"Goods: {city.Goods} Ton";
        approvalText.text = $"Approval: {city.ApprovalRating}%";
        unemployedText.text = $"Unemployed: {city.Unemployed}";
    }
}