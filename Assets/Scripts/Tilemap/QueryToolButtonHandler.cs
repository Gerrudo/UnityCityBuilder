using UnityEngine;
using UnityEngine.UI;

public class QueryToolButtonHandler : MonoBehaviour
{
    private Button button;

    private QueryTool queryTool;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
        queryTool = QueryTool.GetInstance();
    }

    private void ButtonClicked()
    {
        queryTool.IsQueryToolActive = true;
    }
}