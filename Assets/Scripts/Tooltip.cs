using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;

    public TextMeshProUGUI bodyField;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    private void Update()
    {
        int headerLength = headerField.text.Length;
        int bodyLength = bodyField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || bodyLength > characterWrapLimit) ? true : false;
    }
}