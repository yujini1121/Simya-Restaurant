using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveNpcPotion : InteractiveObjectBase
{
    [SerializeField] GameObject nextChat;
    [SerializeField] GameObject nextChatBad;
    [SerializeField] GameObject nextChatGood;
    [SerializeField] Transform chatStartTarget;

    int requireItemId = 200; // 약초 뿌리 아이템 아이디입니다.

    public override void DoInteractiveWithThis()
    {
        GameObject prefab = null; // = (DataController.instance.Access().quests) ? nextChatBad : nextChat;
        if (DataController.instance.Access().quests == false)
        {
            prefab = nextChat;
        }
        else if (DataController.instance.IsExist(200, 3))
        {
            prefab = nextChatGood;
            DataController.instance.Access().quests = false;
            DataController.instance.TryRemoveItem(200, 3);
            DataController.instance.Access().potionsRemain++;
            PlayerHealthUI.instance.UpdatePotionCount(DataController.instance.Access().potionsRemain);
        }
        else
        {
            prefab = nextChatBad;
        }


        PlayerController.instance.PausePlayer();
        GameObject chatBox = Instantiate(prefab);
        chatBox.transform.position = transform.position + new Vector3(0, 3, 0);
        chatBox.transform.SetParent(chatStartTarget);
        
    }
}
