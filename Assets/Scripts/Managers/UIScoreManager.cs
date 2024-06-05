using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScoreManager : MonoBehaviour
{
    public TextMeshProUGUI TotalScore;
    public TextMeshProUGUI PizzaScore;
    public TextMeshProUGUI PizzaNumber;
    public TextMeshProUGUI PizzaIngredient;

    public static UIScoreManager Instance;
    public List<TextMeshProUGUI> pizzaLabels;
    public int lastLabel = 0;
    //Used to create a singleton instance and set it to dont destroy on load
    private void Awake()
    {
        Instance = this;
    }

    public static void UpdateTotalScore(int newScore)
    {
        Instance.TotalScore.text = newScore.ToString();
    }
    public static void AddPizzaIngredient(PizzaIngredient ingredient, Color c)
    {
        
        if (Instance.lastLabel < Instance.pizzaLabels.Count)
        {
            string lab = "";
            switch (ingredient)
            {
                case global::PizzaIngredient.EMPTY:
                    break;
                case global::PizzaIngredient.DOUGH:
                    lab = "dough";
                    break;
                case global::PizzaIngredient.SAUCE:
                    lab = "sauce";
                    break;
                case global::PizzaIngredient.CHEESE:
                    lab = "cheese";
                    break;
                case global::PizzaIngredient.SAUSAGE:
                    lab = "sausage";
                    break;
                case global::PizzaIngredient.BASIL:
                    lab = "basil";
                    break;
                case global::PizzaIngredient.PINEAPPLE:
                    lab = "PINEAPPLE";
                    break;

            }
            Instance.pizzaLabels[Instance.lastLabel].text = lab;
            Instance.pizzaLabels[Instance.lastLabel].color = c;
            Instance.lastLabel++;
        }
    }
    public static void ResetPizzaLabels()
    {
        foreach(TextMeshProUGUI  tmpromesh in Instance.pizzaLabels)
        {
            tmpromesh.text = "";
        }
        Instance.lastLabel = 0;
    }
    public static void UpdatePizzaScore(int newScore)
    {
        Instance.PizzaScore.text = newScore.ToString();
    }

    public static void UpdatePizzaNumber(int newPizzaNumber)
    {
        Instance.PizzaNumber.text = newPizzaNumber.ToString();
    }
    public static void UpdatePizzaIngredient(int ingredientNumber)
    {
        Instance.PizzaIngredient.text = $"{ingredientNumber} / 5";
    }
    public static void ResetLabels() {
        UpdatePizzaIngredient(0);
        UpdatePizzaNumber(1);
        UpdatePizzaScore(0);
        UpdateTotalScore(0);
        ResetPizzaLabels();
    }
}
