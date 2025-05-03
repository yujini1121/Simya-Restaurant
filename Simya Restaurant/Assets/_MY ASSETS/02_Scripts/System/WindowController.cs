using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowController : MonoBehaviour
{
    public bool hasStopTrigger;

    private TextMeshProUGUI m_TextMeshPro;

    public void ChangeLog(string message)
    {
        if (m_TextMeshPro == null)
        {
            m_TextMeshPro = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        if (transform.GetChild(0) == null)
        {
            Debug.Log("비어 있음!");
        }
        if (m_TextMeshPro == null)
        {
            Debug.Log("비어 있음!");
        }

        m_TextMeshPro.text = message;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_TextMeshPro = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
}
