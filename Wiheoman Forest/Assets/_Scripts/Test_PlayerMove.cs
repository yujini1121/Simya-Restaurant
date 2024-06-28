using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Test_PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    [SerializeField] bool _dameged;
    public bool dameged
    {
        get { return _dameged; }
        set { _dameged = value; }
    }

    [SerializeField] bool invincible;
    [SerializeField] float invincibleDuration;

    Slider healthBar;

    Rigidbody rb;

    private void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
        rb = GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        float moveDir = Input.GetAxisRaw("Horizontal");
        //transform.position += new Vector3(moveDir * moveSpeed * Time.deltaTime, 0, 0);
        rb.AddForce(new Vector3(moveDir, 0f, 0f) * moveSpeed);
        

        if (_dameged && !invincible)
        {
            healthBar.value -= 0.1f;
            StartCoroutine(Invincible());
        }
    }

    /// <summary>
    /// 일정 시간동안 무적판정임을 알려주기 위해 색 변경하는 코루틴
    /// </summary>
    IEnumerator Invincible()
    {
        invincible = true;
        Color objColor = GetComponent<MeshRenderer>().material.color;
        GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 0.2f);

        yield return new WaitForSeconds(invincibleDuration - 0.2f);
        GetComponent<MeshRenderer>().material.color = objColor;

        yield return new WaitForSeconds(0.2f);
        invincible = false;
    }
}