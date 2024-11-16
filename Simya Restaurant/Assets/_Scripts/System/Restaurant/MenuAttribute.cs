using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Menu", menuName = "Add/Menu")]
public class MenuAttribute : ScriptableObject
{
    [Header("메뉴 ID (중복 X)")]
    [SerializeField] private int mMenuID;
    public int ItemID
    {
        get
        {
            return mMenuID;
        }
    }

    [Header("메뉴 이름")]
    [SerializeField] private string mMenuName;
    public string ItemName
    {
        get
        {
            return mMenuName;
        }
    }

    [Header("메뉴 이미지")]
    [SerializeField] private Sprite mMenuImage;
    public Sprite ItemImage
    {
        get
        {
            return mMenuImage;
        }
    }
}
