using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [Header("Customer State")]
    [SerializeField] private bool isWaiting = true;
    [SerializeField] private int happiness = 100;
    [SerializeField] private float tipPercentage;
    [SerializeField] private float tipModifier = 1f;

    [Space(10)][Header("Order")]
    [SerializeField] private GameObject orderBubble;
    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private Image orderImage;
    [SerializeField] private ItemAttribute[] menus;

    [Space(10)][Header("Quality")]
    [SerializeField] private string quality;


    private void Start()
    {
        Enter();
        StartCoroutine(DecreaseHappinessOverTime());
    }

    private void Enter()
    {
        string[] entryLines = { "안녕하세요~", "으.. 배고파 죽을뻔 했네요" };
        string random = entryLines[Random.Range(0, entryLines.Length)];
        orderText.text = random;

        float randomCount = Random.Range(2, 10);
        Invoke("OrderMenu", randomCount);
    }

    /// <summary>
    /// 기획 문서와 다름, 우선 빠른 구현을 위해 행복도가 50 미만이면 기존보다 50%의 돈만 받는 것으로 함 
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecreaseHappinessOverTime()
    {
        while (isWaiting && happiness > 0)
        {
            yield return new WaitForSeconds(1f);
            happiness -= 1;

            if (happiness <= 50)
            {
                tipModifier = 0.5f;
            }
        }
    }

    private void OrderMenu()
    {
        isWaiting = false;
        orderImage.gameObject.SetActive(true);

        ItemAttribute[] menuType = { menus[0], menus[1], menus[2] };
        int random = Random.Range(0, 3);

        orderText.text = menuType[random].ItemName + "(으)로 주세요!";
        orderImage.sprite = menuType[random].ItemImage;

        //string[] menuLines = { menus[0].ItemName, menus[1].ItemName, menus[2].ItemName };
        //string random = menuLines[Random.Range(0, menuLines.Length)];
        //orderText.text = random + "(으)로 주세요!";
        //orderImage.sprite = random
    }

    public void ServeMenu()
    {
        switch (quality)
        {
            case "good":
                tipPercentage = Random.Range(0.2f, 0.3f) * tipModifier;
                GiveTip();
                break;
            case "normal":
                tipPercentage = 0f;
                GiveTip();
                break;
            case "bad":
                tipPercentage = -0.5f * tipModifier;
                GiveTip();
                break;
        }

        Invoke("Exit", 3f);
    }

    private void GiveTip()
    {
        float menuPrice = 3000f;
        float tip = menuPrice * tipPercentage;
        int clampTip = Mathf.FloorToInt(tip);
        float finalPrice = clampTip + menuPrice;

        orderText.text = "여기요! +" + finalPrice;
    }

    private void Exit()
    {
        string exitLine = "";

        switch (quality)
        {
            case "good":
                exitLine = "최고의 식사였어요!";
                break;
            case "normal":
                exitLine = "수고하세요~";
                break;
            case "bad":
                exitLine = "별점 테러할게요;;";
                break;
        }
        orderText.text = exitLine;

        StartCoroutine(LeaveAfterServing());
    }

    private IEnumerator LeaveAfterServing()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("손님이 떠났습니다.");
        gameObject.SetActive(false);
    }
}
