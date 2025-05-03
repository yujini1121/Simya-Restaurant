using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Menu", menuName = "Add/Menu")]
public class MenuAttribute : ScriptableObject
{
    [Header("메뉴 ID (중복 X)")]
    [SerializeField] private int mMenuID;
    public int MenuID
    {
        get
        {
            return mMenuID;
        }
    }

    [Header("메뉴 이름")]
    [SerializeField] private string mMenuName;
    public string MenuName
    {
        get
        {
            return mMenuName;
        }
    }

    [Header("메뉴 이미지")]
    [SerializeField] private Sprite mMenuImage;
    public Sprite MenuImage
    {
        get
        {
            return mMenuImage;
        }
    }

    [Header("메뉴 가격")]
    [SerializeField] private float mMenuPrice;
    public float MenuPrice
    {
        get
        {
            return mMenuPrice;
        }
    }
}
