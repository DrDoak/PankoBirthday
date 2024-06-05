using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int StartingDifficultyIndex;
    public int StartingPizzaLevel;
    public Transform MinXSpawnPos;
    public Transform MaxXSpawnPos;

    public List<SpawnRateInfo> allSpawnRates;
    public SpawnRateInfo currentSpawnRate;
    public int currentPizzaDifficultyIndex;
    

    public GameObject DoughTopping;
    public GameObject SauceTopping;
    public GameObject CheeseTopping;
    public List<GameObject> ExtraToppings = new List<GameObject>();

    private List<GameObject> AllCurrentToppings = new List<GameObject>();
    private Dictionary<PizzaRate, float> currentPizzaRates = new Dictionary<PizzaRate, float>();
    private Dictionary<PizzaRate, float> modifyList = new Dictionary<PizzaRate, float>();
    public static SpawnManager Instance;

    //Used to create a singleton instance and set it to dont destroy on load
    private void Awake()
    {
        Instance = this;
        OnGameReset();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        modifyList.Clear();
        foreach (PizzaRate r in currentPizzaRates.Keys)
        {
            float timeForNextSpawn = currentPizzaRates[r];
            if (Time.timeSinceLevelLoad > timeForNextSpawn)
            {
                GameObject nextItemPrefab = GetNextItemToSpawn(r);
                GameObject nextItem = Instantiate(nextItemPrefab);
                Topping tp = nextItem.GetComponent<Topping>();
                tp.YSpeed *= Random.Range(r.speedScaleRange.x, r.speedScaleRange.y);
                tp.homing = r.homing;
                tp.sine = r.sine;
                tp.sineFrequency = r.SineFrequency;
                tp.XSpeed *= r.XSpeedScale;
                
                float xPos = Random.Range(MinXSpawnPos.position.x, MaxXSpawnPos.position.x);
                if (r.useFixedSpawnRange)
                {
                    xPos = Random.Range(r.fixedSpawnRange.x, r.fixedSpawnRange.y);
                }
                nextItem.transform.position = new Vector3(xPos, MinXSpawnPos.position.y, 0);
                AllCurrentToppings.Add(nextItem);
                modifyList[r] = Time.timeSinceLevelLoad + Random.Range(r.delayBetweenSpawns.x, r.delayBetweenSpawns.y);
            }
        }
        foreach(PizzaRate r in modifyList.Keys)
        {
            currentPizzaRates[r] = modifyList[r];
        }
    }
    public static void DeregisterItem(GameObject item)
    {
        if (Instance.AllCurrentToppings.Contains(item))
        {
            Instance.AllCurrentToppings.Remove(item);
        }
    }
    public static void UpdatePizzaLevel(int pizzasCreated) {
        if (pizzasCreated > Instance.currentSpawnRate.MaxPizzaLevel)
        {
            Instance.currentPizzaDifficultyIndex++;
            if (Instance.currentPizzaDifficultyIndex < Instance.allSpawnRates.Count)
            {
                Instance.SetNewSpawnRateInfo(Instance.allSpawnRates[Instance.currentPizzaDifficultyIndex]);
            }
        }
    }
    public GameObject GetNextItemToSpawn(PizzaRate nextSpawn)
    {
        switch (nextSpawn.dropType)
        {
            case DropType.GAMEOBJECT:
                return nextSpawn.objectReference;
            case DropType.NEXT_TOPPING:
                return NextTopping();
            case DropType.RANDOM_TOPPING:
                return RandomTopping((int)PizzaIngredient.DOUGH);
        }
        return CheeseTopping;
    }
    private GameObject NextTopping()
    {
        if (GameProgressManager.currentPizzaStep < PizzaIngredient.CHEESE)
        {
            switch (GameProgressManager.currentPizzaStep)
            {
                case PizzaIngredient.EMPTY:
                    return DoughTopping;
                case PizzaIngredient.DOUGH:
                    return SauceTopping;
                case PizzaIngredient.SAUCE:
                    return CheeseTopping;
            }
        }
        else
        {
            return RandomTopping((int)PizzaIngredient.SAUCE);
        }
        return CheeseTopping;
    }
    private GameObject RandomTopping(int startingPoint)
    {
        int next = Random.Range(startingPoint, (int)PizzaIngredient.SAUSAGE);
        if (next <= (int)PizzaIngredient.CHEESE)
        {
            switch ((PizzaIngredient)next)
            {
                case PizzaIngredient.EMPTY:
                    return DoughTopping;
                case PizzaIngredient.DOUGH:
                    return DoughTopping;
                case PizzaIngredient.SAUCE:
                    return SauceTopping;
                case PizzaIngredient.CHEESE:
                    return CheeseTopping;
            }
        }
        else
        {
            return ExtraToppings[Random.Range(0, ExtraToppings.Count)];
        }

        return CheeseTopping;
    }
    public static void OnGameReset()
    {
        for (int i = Instance.AllCurrentToppings.Count - 1; i >= 0; i--)
        {
            Destroy(Instance.AllCurrentToppings[i]);
        }
        UpdatePizzaLevel(Instance.StartingPizzaLevel);
        Instance.SetNewSpawnRateInfo(Instance.allSpawnRates[Instance.StartingDifficultyIndex]);
        Instance.currentPizzaDifficultyIndex = 0;
    }

    private void SetNewSpawnRateInfo(SpawnRateInfo newSpawnInfo)
    {
        currentSpawnRate = newSpawnInfo;
        currentPizzaRates.Clear();
        foreach (PizzaRate pr in currentSpawnRate.allPossibleDrops)
        {
            currentPizzaRates[pr] = Time.timeSinceLevelLoad + Random.Range(pr.delayBetweenSpawns.x, pr.delayBetweenSpawns.y);
        }
    }
}
