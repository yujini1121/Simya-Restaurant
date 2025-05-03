using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public System.Action lightAttackFunction = () => { };
    public System.Action heavyAttackFunction = () => { };

    private void OnTriggerEnter(Collider other)
    {
        PlayerAttackRange playerAttack = other.GetComponent<PlayerAttackRange>();

        if (playerAttack == null)
        {
            return;
        }

        switch (playerAttack.GetAttackType())
        {
            case EPlayerAttackType.lightAttack:
                lightAttackFunction();
                break;
            case EPlayerAttackType.heavyAttack:
                heavyAttackFunction();
                break;
            default:
                break;
        }
    }
}
