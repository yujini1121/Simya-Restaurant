using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Menu", menuName = "Add/Menu")]
public class MenuAttribute : ScriptableObject
{
    [Header("�޴� ID (�ߺ� X)")]
    [SerializeField] private int mMenuID;
    public int MenuID
    {
        get
        {
            return mMenuID;
        }
    }

    [Header("�޴� �̸�")]
    [SerializeField] private string mMenuName;
    public string MenuName
    {
        get
        {
            return mMenuName;
        }
    }

    [Header("�޴� �̹���")]
    [SerializeField] private Sprite mMenuImage;
    public Sprite MenuImage
    {
        get
        {
            return mMenuImage;
        }
    }

    [Header("�޴� ����")]
    [SerializeField] private float mMenuPrice;
    public float MenuPrice
    {
        get
        {
            return mMenuPrice;
        }
    }
}
