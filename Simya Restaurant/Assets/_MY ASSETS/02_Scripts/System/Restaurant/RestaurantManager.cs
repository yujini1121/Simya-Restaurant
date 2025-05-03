using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantManager : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Image totalPanel;

    [SerializeField] private TextMeshProUGUI dangerFruitTartPriceText;
    [SerializeField] private TextMeshProUGUI flowerTeaPriceText;
    [SerializeField] private TextMeshProUGUI slimeJamBreadText;

    [SerializeField] private TextMeshProUGUI totalPriceText;


    public void CloseRestaurant()
    {
        totalPanel.gameObject.SetActive(!totalPanel.gameObject.activeSelf);

        StartCoroutine(DisplayPricesCoroutine());

    }

    private IEnumerator DisplayPricesCoroutine()
    {
        dangerFruitTartPriceText.text = "";
        flowerTeaPriceText.text = "";
        slimeJamBreadText.text = "";
        totalPriceText.text = "";

        dangerFruitTartPriceText.text = $"�ܰſ��� Ÿ��Ʈ: {CustomerManager.instance.dangerFruitTartTotalPrice}��";
        yield return new WaitForSeconds(1f);  

        flowerTeaPriceText.text = $"����: {CustomerManager.instance.flowerTeaTotalPrice}��";
        yield return new WaitForSeconds(1f);  

        slimeJamBreadText.text = $"�������� �Ļ�: {CustomerManager.instance.slimeJamBreadTotalPrice}��";
        yield return new WaitForSeconds(1f); 

        totalPriceText.text = $"Total: {CustomerManager.instance.totalPrice}��";
    }
}
