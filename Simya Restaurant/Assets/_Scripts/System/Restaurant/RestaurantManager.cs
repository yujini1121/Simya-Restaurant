using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantManager : MonoBehaviour
{
    [SerializeField] Button closeButton;
    [SerializeField] Image TotalPanel;
    [SerializeField] TextMeshProUGUI totalPriceText;


    public void CloseRestaurant()
    {
        totalPriceText.text = $"Total: {Customer.instance.totalPrice}¿ø";
        print($"Total Price: {Customer.instance.totalPrice}¿ø");
    }
}
