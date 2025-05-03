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
    [SerializeField] private string[] goodExitLines = { "�ְ��� �Ļ翴���!", "Ī����ƼĿ 100�� �帱�Կ�!" };
    [SerializeField] private string[] standardExitLines = { "�����ϼ���", "�� �Ծ����ϴ�" };
    [SerializeField] private string[] badExitLines = { "���� �׷��ҰԿ�", "��� �̷��� ����������", "ơ��, ���� �ٽ� ���� ����" };


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
        string[] entryLines = { "�ȳ��ϼ���~", "����� ������ �߾��..", "�� �Դ��ɷ�... ��? �𸣰ڴٱ���?" };
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

        orderText.text = $"{selectedMenu.MenuName}(��)�� �ּ���!";
        orderImage.sprite = selectedMenu.MenuImage;
        orderPrice = selectedMenu.MenuPrice;
    }


    /// <summary>
    /// Serve Menu ��ư���� ����Ǿ� ����. case���� EFoodRank.None�� �ְ� �;�����, �׷��� �ϰ� �Ǹ� ����� ��Ȯ�� �̷������ ����. �������� ����� if�� �ϳ� �� �޾���.
    /// </summary>
    public void ServeMenu()
    {
        if (foodRank == EFoodRank.None)
        {
            Debug.LogError("EFoodRank�� �������� �ʾҽ��ϴ�. �⺻�� Bad�� �����˴ϴ�.");
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
        string[] reactions = { "�ȳ�..", "����..", "��.." };
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
            Debug.LogError("EFoodRank�� �������� �ʾҽ��ϴ�. �⺻�� Bad�� �����˴ϴ�.");
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
        orderText.text = $"�����! +{finalPrice}";

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

        Debug.Log("�մ��� �������ϴ�.");
        gameObject.SetActive(false);
    }

    private void ShowExitReaction()
    {
        string[] exitLines = foodRank switch
        {
            EFoodRank.Good => goodExitLines,
            EFoodRank.Standard => standardExitLines,
            EFoodRank.Bad => badExitLines,
            _ => throw new System.Exception("EFoodRank�� ����� �����Ǿ����� �ʽ��ϴ�.")
        };

        orderText.text = exitLines[Random.Range(0, exitLines.Length)];
    }

    public void UseSpecialItem()
    {
        if (PlayerData.instance.items.Contains("Candy"))
        {
            Debug.Log("�˻��� - �ູ�� +20");

            happiness = Mathf.Min(100, happiness + 20);
        }

        if (PlayerData.instance.items.Contains("MagicPowder"))
        {
            Debug.Log("������ ���� - �丮 ��ũ +1 (Good�϶� ����)");

            if (foodRank == EFoodRank.Standard) { foodRank = EFoodRank.Good; }
            else if (foodRank == EFoodRank.Bad) { foodRank = EFoodRank.Standard; }
        }

        if (PlayerData.instance.items.Contains("CelebrityAutographs"))
        {
            Debug.Log("�������� ���� - �� +5%");

            tipModifier += 0.05f;
        }
    }
}