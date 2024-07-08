using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EPlayerAttackType
{
    none = 0,
    lightAttack = 1,
    heavyAttack = 2
}

public struct PlayerAttackParameters
{
    public EPlayerAttackType attackType;
    public float damage;
    public Vector3 knockbackDirection;
    public float knockbackForce;
}

/// <summary>
///     적 캐릭터에게 데미지를 주는 존재입니다.
/// </summary>
public class PlayerAttackRange : MonoBehaviour
{
    [SerializeField] protected float remainSecond;
    [SerializeField] protected float damage;
    [SerializeField] protected float knockback;
    [SerializeField] protected bool isStunEnemy;
    [SerializeField] protected float stunTime;
    /// <summary>
    ///     해당 옵션이 켜져 있는 경우, 해당 공격은 적 캐릭터의 방어 행위를 무시하고 공격을 적용합니다.
    /// </summary>
    [SerializeField] protected bool isBypassBlock;
    [SerializeField] protected EPlayerAttackType attackType;

    // Start is called before the first frame update
    void Start()
    {
        // 만약 성능이 너무 열악해서 Destroy가 바로 작동하지 않는다면, remainSecond 이후에 BoxCollider의 Active를 해제하세요.
        Destroy(gameObject, remainSecond);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
        if (enemy == null) { return; }
        Vector3 direction = enemy.transform.position - transform.position;
        direction.y = 0;

        enemy.BeAttacked(new PlayerAttackParameters()
        {
            attackType = attackType,
            damage = damage,
            knockbackDirection = direction,
            knockbackForce = knockback,
        });
        if (isStunEnemy)
        {
            enemy.ApplyStun(stunTime);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3[] verts = new Vector3[4];
        verts[0] = new Vector3(
                transform.position.x + transform.localScale.x / 2,
                transform.position.y + transform.localScale.y / 2,
                transform.position.z
            );
        verts[1] = new Vector3(
                transform.position.x + transform.localScale.x / 2,
                transform.position.y - transform.localScale.y / 2,
                transform.position.z
            );
        verts[2] = new Vector3(
                transform.position.x - transform.localScale.x / 2,
                transform.position.y - transform.localScale.y / 2,
                transform.position.z
            );
        verts[3] = new Vector3(
                transform.position.x - transform.localScale.x / 2,
                transform.position.y + transform.localScale.y / 2,
                transform.position.z
            );
        Handles.DrawSolidRectangleWithOutline(verts, new Color(0.5f, 0.5f, 0.5f, 0.1f), Color.black);
    }
}
