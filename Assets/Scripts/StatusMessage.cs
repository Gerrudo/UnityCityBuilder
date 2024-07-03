using TMPro;
using UnityEngine;

public class StatusMessage : MonoBehaviour
{
    public TextMeshProUGUI message;

    private float timeToAppear = 2f;
    private float timeWhenDissapear;

    private void Update()
    {
        if (message.enabled && (Time.time >= timeWhenDissapear))
        {
            message.enabled = false;
        }
    }

    public void UpdateStatusMessage(string newText)
    {
        message.enabled = true;

        message.text = newText;

        timeWhenDissapear = Time.time + timeToAppear;
    }
}