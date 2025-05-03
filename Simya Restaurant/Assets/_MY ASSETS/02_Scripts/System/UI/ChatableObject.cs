using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatableObject : MonoBehaviour
{
    // 네스티드 클래스는 클래스 내부에만 쓰일 클래스를 위해 존재합니다.
    [Serializable]
    protected class NextChatTuple
    {
        public GameObject prefab;
        public string parentNameInRuntime;
    }
    [Serializable]
    protected class NextChatList
    {
        public List<NextChatTuple> list;
    }

    static protected Vector3 globalOffset = new Vector3(0, 4, 0);
    [SerializeField] protected List<NextChatList> nextChats;
    [SerializeField] protected List<TextMeshProUGUI> options;
    protected int index;
    private bool justOnceTrigger = true;

    // Update is called once per frame
    void Update()
    {
        m_Select();
        m_Enter();
        JustOnceTrigger();
    }

    virtual protected void SelectHandler()
    {

    }

    virtual protected int GetSecondIndexNextChat(int index)
    {
        return 0;
    }

    protected void m_Select()
    {
        float input = Input.GetAxis("Horizontal");

        if (Mathf.Abs(input) < 0.05)
        {
            return;
        }

        int delta = (input > 0) ? 1 : 0;
        delta = (input < 0) ? -1 : delta;
        index = Mathf.Clamp(index + delta, 0, nextChats.Count - 1);
        m_SetOptions();
        SelectHandler();
    }

    protected void m_Enter()
    {
        if (Input.GetKeyDown(KeyCode.Return) == false)
        {
            return;
        }

        if (nextChats.Count == 0)
        {
            PlayerController.instance.ResumePlayer();
            Destroy(gameObject);
            return;
        }

        int firstIndex = index;
        int secondIndex = GetSecondIndexNextChat(index);

        if (nextChats[firstIndex].list[secondIndex].prefab != null)
        {
            Transform m_TargetParent = GameObject.Find(nextChats[firstIndex].list[secondIndex].parentNameInRuntime).transform;
            GameObject m_instantiated = Instantiate(nextChats[firstIndex].list[secondIndex].prefab);
            m_instantiated.transform.position = m_TargetParent.position + globalOffset;
            m_instantiated.transform.SetParent(m_TargetParent);
        }
        else
        {
            PlayerController.instance.ResumePlayer();
        }

        Destroy(gameObject);
    }

    protected void JustOnceTrigger()
    {
        if (justOnceTrigger == false)
        {
            return;
        }
        justOnceTrigger = false;

        m_SetOptions();
    }

    private void m_SetOptions()
    {
        if (index >= options.Count)
        {
            return;
        }

        for (int optionsIndex = 0; optionsIndex < options.Count; optionsIndex++)
        {
            options[optionsIndex].fontStyle = FontStyles.Normal;
        }
        options[index].fontStyle = FontStyles.Bold | FontStyles.Underline;
    }
}
