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

	[Space(10)] 
	[Header("Food Rank")]
	[SerializeField] private EFoodRank foodRank;

	[Space(10)] 
	[Header("Order")]
	[SerializeField] private Image orderImage;
	[SerializeField] private TextMeshProUGUI orderText;
	[SerializeField] private TextMeshProUGUI reactionText;

	[Space(10)] 
	[Header("Menu")]
	[SerializeField] private float menuPrice = 3000f;
	[SerializeField] private ItemAttribute[] menus;

	[Space(10)] 
	[Header("Set Exit Lines")]
	[SerializeField] private string[] goodExitLines = { "�ְ��� �Ļ翴���!", "Ī����ƼĿ 100�� �帱�Կ�!" };
	[SerializeField] private string[] standardExitLines = { "�����ϼ���", "�� �Ծ����ϴ�" };
	[SerializeField] private string[] badExitLines = { "���� �׷��ҰԿ�", "��� �̷��� ����������", "ơ��, ���� �ٽ� ���� ����" };


    private void OnEnable()
    {
        Debug.Log("Customer ������Ʈ Ȱ��ȭ��");
    }

    private void OnDisable()
    {
        Debug.LogWarning($"Customer ������Ʈ�� ��Ȱ��ȭ�Ǿ����ϴ�. ȣ�� ��ġ: {System.Environment.StackTrace}");
    }


    private void Start()
	{
        Debug.Log($"Start: Ȱ�� ���� = {gameObject.activeSelf}");

        Enter();
		StartCoroutine(DecreaseHappiness());

        StartCoroutine(TestCoroutine());

    }

    private void Enter()
	{
		string[] entryLines = { "�ȳ��ϼ���~", "����� ������ �߾��..", "�� �Դ��ɷ�... ��? �𸣰ڴٱ���?" };
		orderText.text = entryLines[Random.Range(0, entryLines.Length)];

		Invoke("OrderMenu", Random.Range(2f, 10f));
	}

    private IEnumerator TestCoroutine()
    {
        Debug.Log("TestCoroutine ���۵�");
        yield return new WaitForSeconds(1f);
        Debug.Log("TestCoroutine �Ϸ�");
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
		orderText.text = $"{menus[random].ItemName}(��)�� �ּ���!";
		orderImage.sprite = menus[random].ItemImage;
	}

    private bool isExiting = false;

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

        Debug.Log($"ServeMenu: Ȱ�� ���� = {gameObject.activeSelf}, Hierarchy ���� = {gameObject.activeInHierarchy}");

        if (gameObject.activeSelf && gameObject.activeInHierarchy)
        {
            Debug.Log("ServeMenu: StartCoroutine ���� �غ��");

            // ���� ��Ȱ��ȭ �� ��Ȱ��ȭ
            gameObject.SetActive(false);
            StartCoroutine(ReenableAndStartEating());
        }
        else
        {
            Debug.LogError("ServeMenu: ������Ʈ Ȱ��ȭ ���°� ���������Դϴ�.");
        }
    }

    private IEnumerator ReenableAndStartEating()
    {
        yield return new WaitForEndOfFrame(); // �� ������ ���
        gameObject.SetActive(true);          // ������ ��Ȱ��ȭ
        Debug.Log("ReenableAndStartEating: ��Ȱ��ȭ �Ϸ�");
        StartCoroutine(nameof(Eating));     // Eating �ڷ�ƾ ȣ��
    }


    private IEnumerator Eating()
	{
        Debug.Log($"Eating ����: Ȱ�� ���� = {gameObject.activeSelf}");

        string[] reactions = { "�ȳ�..", "����..", "��.." };
		Vector3[] positions =
		{
			new Vector3(-200f, -50f, 0f),
			new Vector3(-400f, -100f, 0f),
			new Vector3(-200f, -160f, 0f)
		};

		float eatingTime = Random.Range(10f, 20f);
		float elapsedTime = 0f;
		int reactionIndex = 0;

		orderImage.gameObject.SetActive(false);
		reactionText.gameObject.SetActive(true);

		while (elapsedTime < eatingTime)
		{
			reactionText.text = reactions[reactionIndex];
			reactionText.transform.localPosition = positions[reactionIndex];
			reactionIndex = (reactionIndex + 1) % reactions.Length;

			yield return new WaitForSeconds(1f);
			elapsedTime += 1f;
            Debug.Log($"Eating ���� ��: Ȱ�� ���� = {gameObject.activeSelf}");
        }

		reactionText.gameObject.SetActive(false);
		StartCoroutine(Exit());
	}

	private void SetTipPercentage()
	{
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
        if (isExiting) yield break;
        isExiting = true;

        SetTipPercentage();

        float finalPrice = Mathf.FloorToInt(menuPrice * tipPercentage) + menuPrice;
        orderText.text = $"�����! +{finalPrice}";
        yield return new WaitForSeconds(3f);

        ShowExitReaction();
        yield return new WaitForSeconds(3f);

        Debug.Log("�մ��� �������ϴ�.");
        gameObject.SetActive(false);
        isExiting = false;
    }

    private void ShowExitReaction()
	{
		string[] exitLines = foodRank switch
		{
			EFoodRank.Good => goodExitLines,
			EFoodRank.Standard => standardExitLines,
			EFoodRank.Bad => badExitLines,
			_ => throw new System.Exception("Unknown food rank")
		};

		orderText.text = exitLines[Random.Range(0, exitLines.Length)];
	}
}
