using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPIngredient : Topping
{
    public PizzaIngredient pizzaStep;
    protected override void OnContactPlayer()
    {
        GameProgressManager.AddTopping(pizzaStep);
    }
}
