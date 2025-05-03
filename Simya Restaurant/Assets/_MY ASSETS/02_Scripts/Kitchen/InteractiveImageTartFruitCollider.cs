using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTartFruitCollider : InteractiveImageBase
{
    [SerializeField] GameObject fruitGameObject;
    bool isPlaced = false;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        InteractiveImageTartFruit.instance.targetCollider = this;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (InteractiveImageTartFruit.instance.targetCollider == this)
        {
            InteractiveImageTartFruit.instance.targetCollider = null;
        }
    }
    public void PlaceFruit()
    {
        //fruitGameObject.SetActive(true);
        if (isPlaced == false)
        {
            TartMakingController.instance.AddFruit();
        }
        isPlaced = true;



        //Destroy(gameObject);
    }
}
