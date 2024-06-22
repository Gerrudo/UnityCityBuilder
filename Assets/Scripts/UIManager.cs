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

    [SerializeField] private TextMeshProUGUI statsText;

    [SerializeField] private TextMeshProUGUI incomeText;
    [SerializeField] private TextMeshProUGUI expensesText;

    [SerializeField] private TextMeshProUGUI brickProductionText;

    [SerializeField] private TextMeshProUGUI clayConsumptionText;
    [SerializeField] private TextMeshProUGUI clayProductionText;

    [SerializeField] private TextMeshProUGUI energyConsumptionText;
    [SerializeField] private TextMeshProUGUI energyProductionText;

    [SerializeField] private TextMeshProUGUI coalConsumptionText;
    [SerializeField] private TextMeshProUGUI coalProductionText;

    [SerializeField] private TextMeshProUGUI waterConsumptionText;
    [SerializeField] private TextMeshProUGUI waterProductionText;

    [SerializeField] private TextMeshProUGUI populationText;
    [SerializeField] private TextMeshProUGUI jobsText;

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

    public void UpdateStatsUI()
    {
        //Maybe we could have the UI read a value instead of updating it individually?

        //Update Toolbar
        statsText.text = string.Format("Day: {0}   Money: ${1}   Bricks: {2} Tons   Clay: {3} Tons   Coal: {4} Tons", new object[5] { SimulationManager.current.day, SimulationManager.current.money, SimulationManager.current.bricks, SimulationManager.current.clay, SimulationManager.current.coal });

        //Update Panel
        incomeText.text = $"Income: ${SimulationManager.current.income}";
        expensesText.text = $"Expenses: ${SimulationManager.current.expenses}";

        brickProductionText.text = $"Bricks: {SimulationManager.current.brickProduction} Ton Per Day";

        clayConsumptionText.text = $"Clay Usage: {SimulationManager.current.clayConsumption} Ton Per Day";
        clayProductionText.text = $"Clay Produced: {SimulationManager.current.clayProduction} Ton Per Day";

        energyConsumptionText.text = $"Energy Usage: {SimulationManager.current.energyConsumption}Mw";
        energyProductionText.text = $"Energy Produced: {SimulationManager.current.energyProduction}Mw";

        coalConsumptionText.text = $"Coal Usage: {SimulationManager.current.coalConsumption} Ton Per Day";
        coalProductionText.text = $"Coal Produced: {SimulationManager.current.coalProduction} Ton Per Day";

        waterConsumptionText.text = $"Water Usage: {SimulationManager.current.waterConsumption}Kl";
        waterProductionText.text = $"Water Available: {SimulationManager.current.waterProduction}Kl";

        populationText.text = $"Workers: {SimulationManager.current.population}";
        jobsText.text = $"Jobs Available: {SimulationManager.current.jobs}";
    }

    public void OpenStatsPanel(GameObject statsPanel)
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }
}
