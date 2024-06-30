using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject gameOverPanel;
    private bool gameOver = false;

    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI workersText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI waterText;
    [SerializeField] private TextMeshProUGUI coalText;

    void Awake()
    {
        instance = this;
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

        yield return new WaitForSeconds(5);
    }

    public void StartGameOver()
    {
        if (!gameOver)
        {
            gameOver = true;

            StartCoroutine(GameOverSquence());
        }
    }

    public void UpdateStatsUI()
    {
        //Maybe we could have the UI read a value instead of updating it individually?

        //Update Toolbar
        dayText.text = $"Day: {SimulationManager.instance.day}";

        //Update Panel
        workersText.text = $"Workers: {SimulationManager.instance.workers}";
        moneyText.text = $"Money: ${SimulationManager.instance.money}";
        powerText.text = $"Power: {SimulationManager.instance.power}MW";
        waterText.text = $"Water: {SimulationManager.instance.water}kL";
        coalText.text = $"Coal: {SimulationManager.instance.coal} Ton";
    }

    public void OpenStatsPanel(GameObject statsPanel)
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }
}