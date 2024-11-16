using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private GameObject hpBarGameObject;
    [SerializeField] private GameObject textGameObject;
    private Slider slider;
    private TextMeshProUGUI textMesh;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.instance;
        slider = hpBarGameObject.GetComponent<Slider>();
        textMesh = textGameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController != null)
        {
            slider.value = playerController.health / playerController.maxHealth;
        }
    }
}
