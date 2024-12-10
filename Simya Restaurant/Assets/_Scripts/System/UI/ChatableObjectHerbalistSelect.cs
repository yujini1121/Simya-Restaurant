using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatableObjectHerbalistSelect : ChatableObject
{
    // Update is called once per frame
    void Update()
    {
        m_Select();
        m_Enter();
        JustOnceTrigger();
    }

    protected override int GetSecondIndexNextChat(int index)
    {
        if (index == 1)
        {
            return 0;
        }
        if (index != 0)
        {
            return -1; // 일부러 에러 유도
        }

        if (DataController.instance.IsExist(200, 3))
        {
            DataController.instance.TryRemoveItem(200, 3);
            DataController.instance.Access().potionsRemain++;
            PlayerHealthUI.instance.UpdatePotionCount(DataController.instance.Access().potionsRemain);

            return 0;
        }
        else
        {
            return 1;
        }
    }

    protected override void SelectHandler()
    {

    }
}
