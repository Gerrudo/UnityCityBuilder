using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TooltipSystem tooltipSystem;

    [SerializeField] private string header;
    [SerializeField] private string body;

    private void Awake()
    {
        tooltipSystem = TooltipSystem.GetInstance();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipSystem.ShowTooltip(header, body);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipSystem.HideTooltip();
    }
}
