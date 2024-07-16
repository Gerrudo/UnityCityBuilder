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

    public void ShowTooltip(string header, string body)
    {
        tooltip.SetText(header, body);

        tooltip.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.gameObject.SetActive(false);
    }
}