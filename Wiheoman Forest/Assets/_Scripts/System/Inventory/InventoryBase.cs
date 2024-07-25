using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBase : MonoBehaviour
{
    [SerializeField] protected GameObject inventoryBase;
    [SerializeField] protected GameObject inventoryBasePanel;

    protected void Awake()
    {
        if(inventoryBase.activeSelf)
        {
            inventoryBase.SetActive(false);
        }

        // inventoryBasePanel.GetComponentsInChildren<InventorySlot>();
    }
}
