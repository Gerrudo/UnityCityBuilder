using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TooltipSystem tooltipSystem;

    [SerializeField] private BuildingPreset gameTile;

    private void Awake()
    {
        tooltipSystem = TooltipSystem.GetInstance();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //TODO: See if there's a nicer way to do this
        tooltipSystem.ShowTooltip(gameTile.TileName, $"Cost: ${gameTile.CostToBuild.ToString()}", $"Expenses: ${gameTile.Expenses.ToString()}", gameTile.Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipSystem.HideTooltip();
    }
}
