using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class FlowerMon : MonoBehaviour
{
    enum FlowerMonState { Idle, Charging, Attack, Cooldown }

    [SerializeField]
    FlowerMonState curState = FlowerMonState.Idle;

    [Header("Attack")]
    public float pollenAttackRadius;
    public float attackchargingTime;
    public float attackDuration;
    public float attackCooldown;
    public bool playerInAttackRange;

    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        GetComponent<SphereCollider>().radius = pollenAttackRadius;
    }

    private void Update()
    {
        if (curState == FlowerMonState.Attack && playerInAttackRange)
        {
            player.GetComponent<Test_PlayerMove>().dameged = true;
        }
        else
        {
            player.GetComponent<Test_PlayerMove>().dameged = false;
        }

        #region Debug
        switch (curState)
        {
            case FlowerMonState.Idle:
                Debug.Log($"{name} : 기본 상태");
                break;
            case FlowerMonState.Charging:
                Debug.Log($"{name} : 공격 준비 상태");
                break;
            case FlowerMonState.Attack:
                Debug.Log($"{name} : 공격 상태");
                break;
            case FlowerMonState.Cooldown:
                Debug.Log($"{name} : 쿨타임 상태");
                break;
        }
        #endregion
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (curState == FlowerMonState.Idle)
            {
                StartCoroutine(ChangeNextState(FlowerMonState.Charging));
            }

            playerInAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInAttackRange = false;
        }
    }

    IEnumerator ChangeNextState(FlowerMonState nextState, float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);

        switch (nextState)
        {
            case FlowerMonState.Charging:
                StartCoroutine(ChangeNextState(FlowerMonState.Attack, attackchargingTime));
                break;
            case FlowerMonState.Attack:
                StartCoroutine(ChangeNextState(FlowerMonState.Cooldown, attackDuration));
                break;
            case FlowerMonState.Cooldown:
                StartCoroutine(ChangeNextState(FlowerMonState.Idle, attackCooldown));
                break;
        }
        curState = nextState;
    }

    private void OnDrawGizmos()
    {
        switch (curState)
        {
            case FlowerMonState.Idle:
                Handles.color = Color.green;
                Handles.DrawWireDisc(transform.position, Vector3.forward, pollenAttackRadius);
                break;
            case FlowerMonState.Charging:
                Handles.color = Color.red;
                Handles.DrawWireDisc(transform.position, Vector3.forward, pollenAttackRadius);
                break;
            case FlowerMonState.Attack:
                Handles.color = new Color(1f, 0f, 0f, 0.2f);
                Handles.DrawSolidDisc(transform.position, Vector3.forward, pollenAttackRadius);
                break;
            case FlowerMonState.Cooldown:
                Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
                Handles.DrawSolidDisc(transform.position, Vector3.forward, pollenAttackRadius);
                break;
        }
    }
}