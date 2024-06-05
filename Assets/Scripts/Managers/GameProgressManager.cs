using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PizzaIngredient { EMPTY, DOUGH, SAUCE, CHEESE, PINEAPPLE, BASIL, SAUSAGE}
public class GameProgressManager : MonoBehaviour
{
    public int pizzasCreated = 0;
    public int targetPizzas = 22;
    public static PizzaIngredient currentPizzaStep;
    // Start is called before the first frame update
    public static GameProgressManager Instance;

    [SerializeField]
    private PlayerController currentPlayer;
    [SerializeField]
    private Transform playerStartLocation;
    [SerializeField]
    private GameObject YouWinBanner;

    private List<PizzaIngredient> currentPizza = new List<PizzaIngredient>();

    //Used to create a singleton instance and set it to dont destroy on load
    private void Awake()
    {
        Instance = this;        
    }
    void Start()
    {
        GameReset();
    }
    public static GameObject CurrentPlayer()
    {
        return Instance.currentPlayer.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (currentPlayer.isControllable == false)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GameReset();
            }
        }
    }
    public static void AddTopping(PizzaIngredient nextIngredient)
    {
        int previousScore = PizzaScoreManager.Instance.CalculatePizzaScore(Instance.currentPizza);
        Instance.currentPizza.Add(nextIngredient);
        if (Instance.currentPizza.Count >= 5)
        {
            Instance.FinishPizza();
            int currentScore = PizzaScoreManager.Instance.CalculatePizzaScore(Instance.currentPizza);
            UIScoreManager.UpdatePizzaScore(currentScore);
        } else
        {
            UIScoreManager.UpdatePizzaIngredient(Instance.currentPizza.Count);
            if (currentPizzaStep + 1 == nextIngredient)
            {
                currentPizzaStep = nextIngredient;
            }
            int currentScore = PizzaScoreManager.Instance.CalculatePizzaScore(Instance.currentPizza);
            UIScoreManager.UpdatePizzaScore(currentScore);
            Color c = Color.yellow;
            if (currentScore < previousScore)
            {
                c = Color.red;
            } else if (currentScore > previousScore)
            {
                c = Color.green;
            }
            UIScoreManager.AddPizzaIngredient(nextIngredient, c);
        }          
    }
    private void FinishPizza()
    {
        pizzasCreated++;
        UIScoreManager.UpdatePizzaNumber(pizzasCreated + 1);
        UIScoreManager.UpdatePizzaIngredient(0);
        SpawnManager.UpdatePizzaLevel(pizzasCreated);
        PizzaScoreManager.Instance.AddPizzaScore(Instance.currentPizza);
        currentPizzaStep = PizzaIngredient.EMPTY;
        currentPizza.Clear();
        if (pizzasCreated == targetPizzas)
        {
            OnFinish();
        }
        UIScoreManager.ResetPizzaLabels();
    }

    private void OnFinish()
    {
        currentPlayer.isControllable = false;
        Time.timeScale = 0;
        YouWinBanner.SetActive(true);
    }
    private void GameReset()
    {
        Time.timeScale = 1;
        pizzasCreated = 0;
        currentPizzaStep = PizzaIngredient.EMPTY;
        currentPlayer.isControllable = true;
        currentPlayer.transform.position = new Vector3(playerStartLocation.position.x, playerStartLocation.position.y, 0);
        SpawnManager.OnGameReset();
        PizzaScoreManager.ResetScore();
        UIScoreManager.ResetLabels();
        YouWinBanner.SetActive(false);
    }
}
