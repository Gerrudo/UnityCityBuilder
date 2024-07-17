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
    
    protected override void Awake()
    {
        base.Awake();

        UpdateUI();
    }

    public void UpdateUI()
    {
        //Maybe we could have the UI read a value instead of updating it individually?

        //Update Toolbar
        dayText.text = $"Day: {CityData.Day}";
        populationText.text = $"Population: {CityData.Population}";
        fundsText.text = $"Funds: ${CityData.Funds}";

        if (CityData.Earnings > 0)
        {
            earningsText.text = $"Earnings: ${CityData.Earnings}+";
        }
        else
        {
            earningsText.text = $"Earnings: ${CityData.Earnings}";
        }

        //Update Panel
        powerText.text = $"Power: {CityData.Power}kW";
        waterText.text = $"Water: {CityData.Water}kL";
        goodsText.text = $"Goods: {CityData.Goods} Ton";
        approvalText.text = $"Approval: {CityData.ApprovalRating}%";
        unemployedText.text = $"Unemployed: {CityData.Unemployed}";
    }
}