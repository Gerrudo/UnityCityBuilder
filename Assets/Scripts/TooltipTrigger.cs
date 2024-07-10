using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TooltipSystem tooltipSystem;

    private void Awake()
    {
        tooltipSystem = TooltipSystem.GetInstance();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipSystem.ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipSystem.HideTooltip();
    }
}
