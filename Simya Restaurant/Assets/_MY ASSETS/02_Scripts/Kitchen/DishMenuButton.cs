using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishMenuButton : MonoBehaviour
{
    [SerializeField] string dishName;
    [SerializeField] bool isHidden = false;
    [SerializeField] GameObject cookGUI;
    [SerializeField] TMPro.TextMeshProUGUI text;
    bool isShow = false;

    public void ButtonPressed()
    {
        if (isShow == false) return;

        if (cookGUI != null)
        {
            cookGUI.SetActive(true);
        }
        DishMenu.instance.CloseMenu();
    }

    // Start is called before the first frame update
    void Start()
    {
        isShow = (!isHidden) || DataController.instance.HasRecepie(dishName).isExist;
        if (isShow == false)
        {
            text.text = "?????";
        }
        else
        {
            text.text = dishName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
