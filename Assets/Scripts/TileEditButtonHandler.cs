using UnityEngine;
using UnityEngine.UI;

public class TileEditButtonHandler : MonoBehaviour
{
    [SerializeField] PlaceableTile item;
    Button button;

    TileEditor tileEditor;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
        tileEditor = TileEditor.GetInstance();
    }

    private void ButtonClicked()
    {
        Debug.Log("Button was clicked: " + item.name);
        tileEditor.ObjectSelected(item);
    }
}