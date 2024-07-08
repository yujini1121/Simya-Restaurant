using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EKnockbackDirectionType
{
    none,
    pivot,
    oneDirection
}

public class EnemyAttackRange : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float knockBackForce;
    [SerializeField] protected EKnockbackDirectionType knockbackType;
    [SerializeField] protected Vector3 knockbackDirection;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null)
        {
            return;
        }

        Vector3 direction = knockbackType switch
        {
            EKnockbackDirectionType.pivot => (player.transform.position - (transform.position + knockbackDirection)).normalized,
            EKnockbackDirectionType.oneDirection => knockbackDirection,
            _ => new Vector3(0, 0, 0),
        };
        player.BeAttacked(damage, direction, knockBackForce);
    }
}
