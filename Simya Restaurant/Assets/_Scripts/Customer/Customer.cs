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
	[SerializeField] private GameObject orderBubble;
	[SerializeField] private Image orderImage;
	[SerializeField] private TextMeshProUGUI orderText;
	[SerializeField] private TextMeshProUGUI reactionText;

	[Space(10)] 
	[Header("Menu")]
	[SerializeField] private float menuPrice = 3000f;
	[SerializeField] private ItemAttribute[] menus;

	[Space(10)] 
	[Header("Set Exit Lines")]
	[SerializeField] private string[] goodExitLines = { "최고의 식사였어요!", "칭찬스티커 100개 드릴게요!" };
	[SerializeField] private string[] standardExitLines = { "수고하세요", "잘 먹었습니다" };
	[SerializeField] private string[] badExitLines = { "별점 테러할게요", "장사 이렇게 하지마세요", "퉤퉷, 여길 다신 오나 봐라" };

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
		orderText.text = $"{menus[random].ItemName}(으)로 주세요!";
		orderImage.sprite = menus[random].ItemImage;
	}

	public void ServeMenu()
	{
		if (foodRank == EFoodRank.None)
		{
			Debug.LogError("EFoodRank가 설정되지 않았습니다. 기본값 Bad로 설정됩니다.");
			foodRank = EFoodRank.Bad;
		}

		StartCoroutine(Eating());
	}

	private IEnumerator Eating()
	{
		string[] reactions = { "냠냠..", "쩝쩝..", "음.." };
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
		orderBubble.gameObject.SetActive(false);
		reactionText.gameObject.SetActive(true);

		while (elapsedTime < eatingTime)
		{
			reactionText.text = reactions[reactionIndex];
			reactionText.transform.localPosition = positions[reactionIndex];
			reactionIndex = (reactionIndex + 1) % reactions.Length;

			yield return new WaitForSeconds(1f);
			elapsedTime += 1f;
		}

		orderBubble.gameObject.SetActive(true);
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
		SetTipPercentage();

		float finalPrice = Mathf.FloorToInt(menuPrice * tipPercentage) + menuPrice;
		orderText.text = $"여기요! +{finalPrice}";
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
			_ => throw new System.Exception("Unknown food rank")
		};

		orderText.text = exitLines[Random.Range(0, exitLines.Length)];
	}
}
