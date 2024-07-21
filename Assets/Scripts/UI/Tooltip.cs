using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Loading;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI expensesText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private LayoutElement layoutElement;

    public void SetText(string buildingName, string cost, string expenses, string description)
    {
        buildingNameText.text = buildingName;
        costText.text = cost;
        expensesText.text = expenses;
        descriptionText.text = description;
    }

    private void Update()
    {
        layoutElement.enabled = buildingNameText.preferredWidth > layoutElement.preferredWidth || descriptionText.preferredWidth > layoutElement.preferredWidth;
    }
}