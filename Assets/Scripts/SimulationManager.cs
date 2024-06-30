using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager instance;

    public int day;
    public int money;
    public int power;
    public int water;
    public int workers;
    public int coal;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(CountDays());
    }

    private IEnumerator CountDays()
    {
        yield return new WaitForSeconds(10);

        day++;

        UIManager.instance.UpdateStatsUI();

        StartCoroutine(CountDays());
    }
}