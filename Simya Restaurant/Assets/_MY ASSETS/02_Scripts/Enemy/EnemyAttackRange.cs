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
    [SerializeField] protected List<string> nameOfDamageableTargets; // 어떤 공격은 몬스터에게도 적용이 가능합니다.

    private void OnTriggerEnter(Collider other)
    {
        if (nameOfDamageableTargets.Contains(other.name))
        {
            // 데미지를 가합니다.
            EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();

            enemy.BeAttacked(new PlayerAttackParameters()
            {
                attackType = EPlayerAttackType.none,
                damage = damage,
                knockbackDirection = knockbackDirection,
                knockbackForce = knockBackForce
            });

        }


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
