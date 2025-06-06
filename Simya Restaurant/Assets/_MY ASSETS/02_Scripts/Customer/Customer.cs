using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Customer : MonoBehaviour
{
    public static Customer instance;

    [Header("Customer State")]
    [SerializeField] private bool isWaiting = true;
    [SerializeField] private int happiness = 100;
    [SerializeField] private float tipPercentage;
    [SerializeField] private float tipModifier = 1f;

    [Space(10)]
    [Header("Food Rank")]
    [SerializeField] private EFoodRank foodRank;

    [Space(10)]
    [Header("Order")]
    [SerializeField] private TextMeshProUGUI reactionText;
    [SerializeField] private Image orderImage;
    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private float orderPrice;

    [Space(10)]
    [Header("Menu")]
    [SerializeField] private MenuAttribute[] menus;
    [SerializeField] private MenuAttribute selectedMenu;

    private float finalPrice = 0f;

    [Space(10)]
    [Header("Set Exit Lines")]
    [SerializeField] private string[] goodExitLines = { "최고의 식사였어요!", "칭찬스티커 100개 드릴게요!" };
    [SerializeField] private string[] standardExitLines = { "수고하세요", "잘 먹었습니다" };
    [SerializeField] private string[] badExitLines = { "별점 테러할게요", "장사 이렇게 하지마세요", "퉤퉷, 여길 다신 오나 봐라" };


    private enum items
    {
        Candy = 2001,
        CelebrityAutographs = 2002,
        MagicPoweder = 2003,
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Enter();
        StartCoroutine(DecreaseHappiness());
    }


    private void Enter()
    {
        string[] entryLines = { "안녕하세요~", "배고파 죽을뻔 했어요..", "늘 먹던걸로... 네? 모르겠다구요?" };
        orderText.text = entryLines[Random.Range(0, entryLines.Length)];

        Invoke("OrderMenu", Random.Range(2f, 10f));
    }

    private IEnumerator DecreaseHappiness()
    {
        while (isWaiting && happiness > 0)
        {
            yield return new WaitForSeconds(1f);
            happiness--;

            if (happiness <= 50) tipModifier = 0.5f;
        }
    }

    private void OrderMenu()
    {
        isWaiting = false;
        orderImage.gameObject.SetActive(true);

        int random = Random.Range(0, menus.Length);
        selectedMenu = menus[random];

        orderText.text = $"{selectedMenu.MenuName}(으)로 주세요!";
        orderImage.sprite = selectedMenu.MenuImage;
        orderPrice = selectedMenu.MenuPrice;
    }


    /// <summary>
    /// Serve Menu 버튼으로 연결되어 있음. case문에 EFoodRank.None도 넣고 싶었지만, 그렇게 하게 되면 계산이 정확히 이루어지지 않음. 안전성을 고려해 if문 하나 더 달았음.
    /// </summary>
    public void ServeMenu()
    {
        if (foodRank == EFoodRank.None)
        {
            Debug.LogError("EFoodRank가 설정되지 않았습니다. 기본값 Bad로 설정됩니다.");
            foodRank = EFoodRank.Bad;
        }

        switch (foodRank)
        {
            case EFoodRank.Good:
                tipPercentage = Random.Range(0.2f, 0.3f) * tipModifier;
                break;
            case EFoodRank.Standard:
                tipPercentage = 0f;
                break;
            case EFoodRank.Bad:
                tipPercentage = -0.5f * tipModifier;
                break;
        }
        StartCoroutine(Eating());
    }

    private IEnumerator Eating()
    {
        string[] reactions = { "냠냠..", "쩝쩝..", "음.." };
        Vector3[] positions =
        {
            new Vector3(150f, -50f, 0f),
            new Vector3(-50f, -100f, 0f),
            new Vector3(150f, -160f, 0f)
        };

        float eatingTime = Random.Range(10f, 20f);
        float elapsedTime = 0f;
        int reactionIndex = 0;

        orderImage.gameObject.SetActive(false);
        orderText.gameObject.SetActive(false);
        reactionText.gameObject.SetActive(true);

        while (elapsedTime < eatingTime)
        {
            reactionText.text = reactions[reactionIndex];
            reactionText.transform.localPosition = positions[reactionIndex];
            reactionIndex = (reactionIndex + 1) % reactions.Length;

            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }

        reactionText.gameObject.SetActive(false);
        StartCoroutine(Exit());
    }

    private void SetTipPercentage()
    {
        if (foodRank == EFoodRank.None)
        {
            Debug.LogError("EFoodRank가 설정되지 않았습니다. 기본값 Bad로 설정됩니다.");
            foodRank = EFoodRank.Bad;
        }

        switch (foodRank)
        {
            case EFoodRank.Good:
                tipPercentage = Random.Range(0.2f, 0.3f) * tipModifier;
                break;
            case EFoodRank.Standard:
                tipPercentage = 0f;
                break;
            case EFoodRank.Bad:
                tipPercentage = -0.5f * tipModifier;
                break;
        }
    }

    private IEnumerator Exit()
    {
        SetTipPercentage();
        UseSpecialItem();

        orderText.gameObject.SetActive(true);

        finalPrice = Mathf.FloorToInt(orderPrice * (1 + tipPercentage));
        orderText.text = $"여기요! +{finalPrice}";

        switch (selectedMenu.MenuID)
        {
            case 1:
                CustomerManager.instance.dangerFruitTartTotalPrice += finalPrice;
                Debug.Log("dangerFruitTartTotalPrice : " + CustomerManager.instance.dangerFruitTartTotalPrice);
                break;
            case 2:
                CustomerManager.instance.flowerTeaTotalPrice += finalPrice;
                Debug.Log("flowerTeaTotalPrice : " + CustomerManager.instance.flowerTeaTotalPrice);
                break;
            case 3:
                CustomerManager.instance.slimeJamBreadTotalPrice += finalPrice;
                Debug.Log("slimeJamBreadTotalPrice : " + CustomerManager.instance.slimeJamBreadTotalPrice);
                break;
        }

        CustomerManager.instance.totalPrice += finalPrice;
        Debug.Log("totalPrice : " + CustomerManager.instance.totalPrice);


        yield return new WaitForSeconds(3f);

        ShowExitReaction();
        yield return new WaitForSeconds(3f);

        Debug.Log("손님이 떠났습니다.");
        gameObject.SetActive(false);
    }

    private void ShowExitReaction()
    {
        string[] exitLines = foodRank switch
        {
            EFoodRank.Good => goodExitLines,
            EFoodRank.Standard => standardExitLines,
            EFoodRank.Bad => badExitLines,
            _ => throw new System.Exception("EFoodRank가 제대로 설정되어있지 않습니다.")
        };

        orderText.text = exitLines[Random.Range(0, exitLines.Length)];
    }

    public void UseSpecialItem()
    {
        if (PlayerData.instance.items.Contains("Candy"))
        {
            Debug.Log("알사탕 - 행복도 +20");

            happiness = Mathf.Min(100, happiness + 20);
        }

        if (PlayerData.instance.items.Contains("MagicPowder"))
        {
            Debug.Log("마법의 가루 - 요리 랭크 +1 (Good일때 제외)");

            if (foodRank == EFoodRank.Standard) { foodRank = EFoodRank.Good; }
            else if (foodRank == EFoodRank.Bad) { foodRank = EFoodRank.Standard; }
        }

        if (PlayerData.instance.items.Contains("CelebrityAutographs"))
        {
            Debug.Log("유명인의 싸인 - 팁 +5%");

            tipModifier += 0.05f;
        }
    }
}