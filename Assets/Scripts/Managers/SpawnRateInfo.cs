using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropType { RANDOM_TOPPING, NEXT_TOPPING, GAMEOBJECT}
[System.Serializable]
public class PizzaRate
{
    public float rateRelative = 1f;
    public DropType dropType;
    public GameObject objectReference;
    public Vector2 delayBetweenSpawns;
    public Vector2 speedScaleRange;
    public bool useFixedSpawnRange;
    public Vector2 fixedSpawnRange;
    public bool homing = false;
    public bool sine = false;
    public float SineFrequency = 1;
    public float XSpeedScale = 1;
    
}
[CreateAssetMenu(fileName = "SpawnRate", menuName = "Panko/SpawnGuidelines")]
public class SpawnRateInfo : ScriptableObject
{
    public int MaxPizzaLevel;
    public List<PizzaRate> allPossibleDrops;
    
    public PizzaRate GetNextSpawn()
    {
        float totalRate = TotalSpawnRate();
        float targetRate = Random.Range(0, totalRate);
        float currentAccumulatedRate = 0;
        foreach (PizzaRate rate in allPossibleDrops)
        {
            currentAccumulatedRate += rate.rateRelative;
            if (currentAccumulatedRate > targetRate)
            {
                return rate;
            }
        }
        return allPossibleDrops[0];
    }
    private float TotalSpawnRate()
    {
        float totalRates = 0;
        foreach (PizzaRate rate in allPossibleDrops)
        {
            totalRates += rate.rateRelative;
        }
        return totalRates;
    }
}
