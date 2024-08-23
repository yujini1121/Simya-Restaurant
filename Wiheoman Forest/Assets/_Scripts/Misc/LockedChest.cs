using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedChest : MonoBehaviour
{
    Damageable myDamage;
    bool isOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        myDamage = GetComponent<Damageable>();
        myDamage.heavyAttackFunction =
            () =>
            {
                if (isOpened)
                {
                    return;
                }
                isOpened = true;

                Debug.Log("레시피를 얻었습니다.");

                CanvasController.instance.OpenTextWindow("Opened the box!\nEnter to close", true);
                CanvasController.instance.OpenTextWindow("We got recipe!\nEnter to close", true);
            };
    }
}
