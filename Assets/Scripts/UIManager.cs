using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager current;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI text;
    private bool gameOver = false;

    void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartGameOver();
        }

        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                //Causing issues, may need to alter how we do this or change how the tilemap is displayed 
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
        }
    }

    private IEnumerator GameOverSquence()
    {
        gameOverPanel.SetActive(true);

        yield return new WaitForSeconds(5.0f);
    }

    public void StartGameOver()
    {
        if (!gameOver)
        {
            gameOver = true;

            StartCoroutine(GameOverSquence());
        }
    }
}
