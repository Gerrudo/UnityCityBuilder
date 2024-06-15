using TMPro;
using UnityEngine;

public class StatusMessage : MonoBehaviour
{
    public static StatusMessage current;

    public TextMeshProUGUI message;

    private float timeToAppear = 2f;
    private float timeWhenDissapear;

    void Awake()
    {
        current = this;

        message.enabled = false;
    }

    void Update()
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