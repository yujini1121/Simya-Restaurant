using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

/// <summary>
///     해당 적 캐릭터의 등급을 나타냅니다.
/// </summary>
public enum ERank
{
    empty,
    normal,
    elite,
    boss
}

/// <summary>
///     해당 에너미의 상태를 나타냅니다.
/// </summary>
[Serializable]
public struct EnemyStatus
{
    public float health;
    public int guardGauge;
    public ERank rank;
}

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float range = 0;
    [SerializeField] protected EnemyStatus stat;

    static protected GameObject playerGameObject = null;
    protected Rigidbody enemyRigidbody;

    protected bool isDead = false;
    protected bool isStuned = false;            // 경직 or 스턴용 값
    protected float endStunTime;
    protected Coroutine stunReleaseCoroutine;


    /// <summary>
    ///     해당 씬에 유일하게 존재하는 플레이어의 정보를 에너미에게 알려줍니다.
    /// </summary>
    /// <param name="player"></param>
    static public void SetPlayer(GameObject player)
    {
        playerGameObject = player;
    }

    /// <summary>
    ///     해당 적 캐릭터가 플레이어를 발견했는지 여부를 파악합니다.
    /// </summary>
    /// <returns></returns>
    public bool IsFoundPlayer()
    {
        if (playerGameObject == null) return false;
        return (range * range) > (transform.position - playerGameObject.transform.position).sqrMagnitude;
    }

    /// <summary>
    ///     에너미 위치에서 출발하여 플레이어가 있는 방향으로 바라보는 벡터를 간략하게 리턴해줍니다.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPseudoDirection()
    {
        if (IsFoundPlayer() == false) return Vector3.zero;
        float dx = playerGameObject.transform.position.x - transform.position.x;
        return (dx < 0) ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);
    }

    /// <summary>
    ///     공격을 받은 경우를 설정합니다.
    /// </summary>
    public void BeAttacked(float damage, Vector3 knockBackDirection, float force)
    {
        // ===============================
        // 공격을 받은 경우 해당 함수를 호출합니다.
        // 만약에 추가적인 합의로 인해, 방어력이나 무적 상태 등 피해 매커니즘이 복접해지는 경우, 해당 함수를 수정하세요.
        // 공격 받음 판정을 콜라이더를 활용하여 OnTriggerEnter / OnCollisionEnter 등으로 활용할것인데 이들중 어떤것을 쓸건지도 확실하지 않을 뿐더러
        // 심지어 플레이어의 무기의 콜라이더가 어떤 태그를 가지고 있는지도 몰라서 그냥 함수를 만들었습니다.
        // 결론적으로 나중에 해당 내용이 합의된다면 해당 함수를 호출하세요.
        // ===============================

        stat.health -= damage;

        if (stat.health <= 0.0f && (isDead == false))
        {
            DoDeathHandle();
        }
        if (enemyRigidbody != null)
        {
            enemyRigidbody.AddForce(knockBackDirection.normalized * force, ForceMode.Impulse);
        }
    }

    /// <summary>
    ///     에너미에게 경직(스턴)을 stunTime 만큼 적용시킵니다. 이때, 이미 스턴을 가지고 있다면, 남은 스턴시간과 새 적용 스턴시간 중 최댓값을 적용합니다.
    /// </summary>
    /// <param name="stunTime"></param>
    public void ApplyStun(float stunTime)
    {
        float newEndStunTime = Time.time + stunTime;
        if (isStuned == false)
        {
            isStuned = true;
            endStunTime = newEndStunTime;
            stunReleaseCoroutine = StartCoroutine(ReleaseStunAfterTime(stunTime));
            return;
        }

        isStuned = true;

        if (endStunTime >= newEndStunTime)
        {
            return;
        }

        endStunTime = newEndStunTime;
        StopCoroutine(stunReleaseCoroutine);
        stunReleaseCoroutine = StartCoroutine(ReleaseStunAfterTime(stunTime));
    }

    protected IEnumerator ReleaseStunAfterTime(float time)
    {
        Debug.Log("스턴 적용됨");
        yield return new WaitForSeconds(time);
        isStuned = false;
        Debug.Log("스턴 해제됨");
    }

    /// <summary>
    ///     해당 적 캐릭터의 사망을 처리하는 함수입니다. 반드시 구현해주세요. 사망시 코루틴 삭제 및 삭제 모션 재생 등이 있습니다.
    /// </summary>
    protected abstract void DoDeathHandle();
}
