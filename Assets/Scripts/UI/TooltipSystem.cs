using UnityEngine;

public class TooltipSystem : Singleton<TooltipSystem>
{
    [SerializeField] private Tooltip tooltip;

    protected override void Awake()
    {
        base.Awake();
    }

    public void ShowTooltip(string buildingName, string cost, string expenses, string description)
    {
        tooltip.SetText(buildingName, cost, expenses, description);

        tooltip.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.gameObject.SetActive(false);
    }
}