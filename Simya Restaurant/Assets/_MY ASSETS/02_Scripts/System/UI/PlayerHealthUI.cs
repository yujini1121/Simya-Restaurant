using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public static PlayerHealthUI instance
    {
        get
        {
            if (m_instance == null)
            {
                Debug.LogWarning("PlayerHealthUI의 인스턴스가 null입니다!");
                return null;
            }
            else
            {
                return m_instance;
            }
        }
        private set => m_instance = value;
    }

    [SerializeField] private GameObject hpBarGameObject;
    [SerializeField] private GameObject textGameObject;
    [SerializeField] private GameObject iconGameObject;
    private Slider slider;
    private TextMeshProUGUI textMesh;
    private PlayerController playerController;
    private static PlayerHealthUI m_instance;
    private Image cooltimeImage;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.instance;
        slider = hpBarGameObject.GetComponent<Slider>();
        textMesh = textGameObject.GetComponent<TextMeshProUGUI>();
        cooltimeImage = iconGameObject.GetComponent<Image>();

        UpdatePotionCount(FindObjectOfType<DataController>().Access().potionsRemain);
    }

    private void Update()
    {
        slider.value = playerController.Health / playerController.maxHealth;
        if (DataController.instance != null)
        {
#warning TODO : 플레이어 프리펩 머지 끝나고 하드코딩 수정하기
            cooltimeImage.fillAmount
                = Mathf.Max(0.0f, DataController.instance.Access().cooltimePotion - Time.deltaTime) / 3.0f; // playerController.PotionCoolTime;
        }
        
    }

    public void UpdateHealth()
    {
        
    }

    public void UpdatePotionCount(int count)
    {
        textMesh.text = $"×{count}";
    }
}