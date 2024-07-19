using UnityEngine;
using UnityEngine.UI;

public class TileEditButtonHandler : MonoBehaviour
{
    [SerializeField] private GameTile item;
    private Button button;

    TileEditor tileEditor;

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