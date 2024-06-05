using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaScoreManager : MonoBehaviour
{
    public static PizzaScoreManager Instance;
    private int Score = 0;
    [SerializeField]
    private int DoughScore = 10;
    [SerializeField]

    private int SauceScore = 20;
    [SerializeField]

    private int CheeseScore = 20;
    [SerializeField]

    private int BasilScore = 30;
    [SerializeField]

    private int SausageScore = 30;
    [SerializeField]

    private int PineappleScore = -50;

    //Used to create a singleton instance and set it to dont destroy on load
    private void Awake()
    {
        Instance = this;
    }
    public void AddPizzaScore(List<PizzaIngredient> ingredients)
    {
        int pizzaScore = CalculatePizzaScore(ingredients);
        Score += pizzaScore;
        UIScoreManager.UpdateTotalScore(Score);
    }
    public int CalculatePizzaScore(List<PizzaIngredient> ingredients)
    {
        int score = 0;
        PizzaIngredient nextIngredientTargeting = PizzaIngredient.DOUGH;
        int numPineapple = 0;
        foreach(PizzaIngredient i in ingredients)
        {
            if (i == PizzaIngredient.PINEAPPLE)
            {
                numPineapple++;
                continue;
            }
            if (i == PizzaIngredient.DOUGH)
            {
                score = 0;
            }
            if (nextIngredientTargeting <= PizzaIngredient.CHEESE && i <= nextIngredientTargeting)
            {
                nextIngredientTargeting ++;
                score += GetIngredientValue(i);
            } else if (nextIngredientTargeting > PizzaIngredient.CHEESE)
            {
                score += GetIngredientValue(i);
            }
        }
        if (numPineapple == 0)
        {
            return score;
        } else if (numPineapple == 1)
        {
            return 0;
        } else if (numPineapple == 5)
        {
            return PineappleScore * 10;
        } else
        {
            return PineappleScore * numPineapple;
        }
        return score;
    }
    public int GetIngredientValue(PizzaIngredient ing)
    {
        switch (ing)
        {
            case PizzaIngredient.EMPTY:
                return 0;
            case PizzaIngredient.DOUGH:
                return DoughScore;
            case PizzaIngredient.SAUCE:
                return SauceScore;
            case PizzaIngredient.CHEESE:
                return CheeseScore;
            case PizzaIngredient.PINEAPPLE:
                return PineappleScore;
            case PizzaIngredient.BASIL:
                return BasilScore;
            case PizzaIngredient.SAUSAGE:
                return SausageScore;
        }
        return 0;
    }
    public static void ResetScore()
    {
        Instance.Score = 0;
    }
    
}
