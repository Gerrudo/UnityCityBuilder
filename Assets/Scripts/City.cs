using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class City : Singleton<City>
{
    public int day { get; set; }
    [SerializeField] public int money { get; set; }
    public int power { get; set; }
    public int water { get; set; }
    public int population { get; set; }
    public int coal { get; set; }

    CityStatistics cityStatistics;

    protected override void Awake()
    {
        base.Awake();

        cityStatistics = CityStatistics.GetInstance();
    }

    private void Start()
    {
        StartCoroutine(CountDays());
    }

    private IEnumerator CountDays()
    {
        yield return new WaitForSeconds(10);

        day++;

        cityStatistics.UpdateUI();

        StartCoroutine(CountDays());
    }

    private void GetResidential()
    {

    }
}