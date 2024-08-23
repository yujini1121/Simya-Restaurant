using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingManager : MonoBehaviour
{
    static public CookingManager instance;




    private void Awake()
    {
        instance = this;
    }
}
