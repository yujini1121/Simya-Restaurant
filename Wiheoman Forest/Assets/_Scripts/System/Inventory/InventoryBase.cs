using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBase : MonoBehaviour
{
    [SerializeField] private GameObject playerInventory;
    [SerializeField] private GameObject chestInventory;

    private void Awake()
    {
        if (playerInventory.activeSelf) { playerInventory.SetActive(false); }
        if (chestInventory.activeSelf)  { chestInventory.SetActive(false); }
    }

    private void Update()
    {
        
    }
}
