using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFoodRank
{
    None,
    Good,
    Standard,
    Bad,
}

public class FoodComponent : MonoBehaviour
{
    public string name { get; private set; }
    public int originalPrice { get; private set; }
    public EFoodRank rank
    {
        get
        {
            return m_rank;
        }
        set
        {
            if (rank != EFoodRank.None) return;
            m_rank = value;
        }
    }
    public int price
    {
        get => rank switch
        {
            EFoodRank.Good => originalPrice * 140 / 100,
            EFoodRank.Standard => originalPrice,
            EFoodRank.Bad => originalPrice * 50 / 100,
            _ => -1
        };
    }
    private EFoodRank m_rank;

    public FoodComponent(string _name, int _originalPrice)
    {
        name = _name;
        originalPrice = _originalPrice;
        m_rank = EFoodRank.None;
    }
}
