using UnityEngine;
using UnityEngine.UI;

public class TileEditButtonHandler : MonoBehaviour
{
    [SerializeField] private BuildingPreset item;
    private Button button;

    private TileEditor tileEditor;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
        tileEditor = TileEditor.GetInstance();
    }

    private void ButtonClicked()
    {
        tileEditor.ObjectSelected(item);
    }
}