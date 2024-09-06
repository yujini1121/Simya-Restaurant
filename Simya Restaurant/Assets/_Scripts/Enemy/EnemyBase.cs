using System.Collections;
using System.Collections.Generic;
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
[System.Serializable]
public struct EnemyStatus
{
    public float health;
    public float guardGauge;
    public ERank rank;
}

[System.Serializable]
public struct DropTuple
{
    public float probabilityWeight;
    public int dropCount;
}

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] protected GameObject[] items;
    [SerializeField] protected List<DropTuple> dropRate;

    [Header("Set Value")]
    [SerializeField] protected float range = 0;
    [SerializeField] protected EnemyStatus stat;

    static protected GameObject playerGameObject = null;
    static protected PlayerController playerScript = null;
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
        playerScript = player.GetComponent<PlayerController>();
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
        return (dx < 0) ? Vector3.left : Vector3.right;
    }

    /// <summary>
    ///     공격을 받은 경우를 설정합니다.
    /// </summary>
    public void BeAttacked(PlayerAttackParameters parameter)
    {
        // ===============================
        // 공격을 받은 경우 해당 함수를 호출합니다.
        // 만약에 추가적인 합의로 인해, 방어력이나 무적 상태 등 피해 매커니즘이 복접해지는 경우, 해당 함수를 수정하세요.
        // 공격 받음 판정을 콜라이더를 활용하여 OnTriggerEnter / OnCollisionEnter 등으로 활용할것인데 이들중 어떤것을 쓸건지도 확실하지 않을 뿐더러
        // 심지어 플레이어의 무기의 콜라이더가 어떤 태그를 가지고 있는지도 몰라서 그냥 함수를 만들었습니다.
        // 결론적으로 나중에 해당 내용이 합의된다면 해당 함수를 호출하세요.
        // ===============================

        stat.health -= parameter.damage;
        DoInjuryHandle(parameter);

        if (stat.health <= 0.0f && (isDead == false) && IsDeathAllowed())
        {
            DoDeathHandle();
            isDead = true;
        }
        if (enemyRigidbody != null)
        {
            enemyRigidbody.AddForce(parameter.knockbackDirection.normalized * parameter.knockbackForce, ForceMode.Impulse);
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
        yield return new WaitForSeconds(time);
        isStuned = false;
    }

    /// <summary>
    ///     해당 적 캐릭터의 사망을 처리하는 함수입니다. 반드시 구현해주세요. 사망시 코루틴 삭제 및 삭제 모션 재생 등이 있습니다.
    /// </summary>
    protected abstract void DoDeathHandle();

    /// <summary>
    ///     해당 적 캐릭터의 부상을 처리하는 함수입니다. 체력이 깎이고 나서 호출됩니다.
    /// </summary>
    /// <param name="damage"></param>
    protected virtual void DoInjuryHandle(PlayerAttackParameters parameter)
    {
        
    }

    /// <summary>
    ///     해당 에너미의 사망에 추가적인 조건이 붙은 경우, 해당 함수를 오버라이드하세요.
    /// </summary>
    /// <returns></returns>
    protected virtual bool IsDeathAllowed()
    {
        return true;
    }

    protected void DropItems()
    {
        gameObject.SetActive(false);            // 임의로 비활성화 해둔 것, Die()를 구현하면 삭제해도 상관 없음
        int itemsToDrop = DetermineItemsCount();

        for (int i = 0; i < itemsToDrop; i++)
        {
            int itemIndex = Random.Range(0, items.Length);
            Instantiate(items[itemIndex], transform.position, Quaternion.identity);
        }
    }

#warning TODO : 아이탬 갯수 인스팩터 창을 통해 결정하도록 함 아이탬 갯수 및 확률은 맴버 변수에서 저장
    /// <summary>
    /// 떨어트릴 아이템 개수 랜덤으로 지정하는 메서드 
    /// </summary>
    /// <returns></returns>
    protected int DetermineItemsCount()
    {
        if (dropRate.Count == 0)
        {
            dropRate = new List<DropTuple>()
            {
                new DropTuple()
                {
                    dropCount = 1,
                    probabilityWeight = 0.5f
                },
                new DropTuple()
                {
                    dropCount = 2,
                    probabilityWeight = 0.3f
                },
                new DropTuple()
                {
                    dropCount = 3,
                    probabilityWeight = 0.2f
                },
            };
        }


        float m_sum = 0.0f;
        for (int index = 0; index < dropRate.Count; ++index)
        {
            m_sum += dropRate[index].probabilityWeight;
        }
        float probability = Random.value * m_sum;   // 0.0 ~ m_sum 사이의 임의의 랜덤 숫자 생성

        for (int index = 0; index < dropRate.Count; ++index)
        {
            if (probability < dropRate[index].probabilityWeight)
            {
                return dropRate[index].dropCount;
            }

            probability -= dropRate[index].probabilityWeight;
        }
        return dropRate[dropRate.Count - 1].dropCount;
    }
}
