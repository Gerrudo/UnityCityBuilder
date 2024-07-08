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
        populationText.text = $"Population: {city.Population}";
        fundsText.text = $"Funds: ${city.Funds}";

        if (city.Earnings > 0)
        {
            earningsText.text = $"Funds: ${city.Earnings}+";
        }
        else
        {
            earningsText.text = $"Funds: ${city.Earnings}";
        }

        //Update Panel
        powerText.text = $"Power: {city.Power}MW";
        waterText.text = $"Water: {city.Water}kL";
        goodsText.text = $"Goods: {city.Goods} Ton";
        approvalText.text = $"Approval: {city.ApprovalRating}%";
    }
}