using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchableItem : MonoBehaviour
{
    [SerializeField] public int itemId;
    [SerializeField] public int count;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == PlayerController.instance.gameObject.name)
        {
            UtilityFunctions.Log(this, $"플레이어는 아이템을 획득했습니다! 아이템 아이디 : {itemId}, 아이템 갯수 {count}");

            Destroy(gameObject);
        }
    }
}
