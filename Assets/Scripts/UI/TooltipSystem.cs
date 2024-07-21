using UnityEngine;

public class TooltipSystem : Singleton<TooltipSystem>
{
    TooltipSystem tooltipSystem;

    [SerializeField] private Tooltip tooltip;

    protected override void Awake()
    {
        base.Awake();

        tooltipSystem = TooltipSystem.GetInstance();
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