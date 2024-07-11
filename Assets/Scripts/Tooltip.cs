using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Loading;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI bodyField;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int characterWrapLimit;

    public void SetText(string header, string body)
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        bodyField.text = body;
    }

    private void Update()
    {
        layoutElement.enabled = headerField.preferredWidth > layoutElement.preferredWidth || bodyField.preferredWidth > layoutElement.preferredWidth;
    }
}