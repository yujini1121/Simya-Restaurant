using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        float moveDir = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(moveDir * moveSpeed * Time.deltaTime, 0, 0);

        if (_dameged && !invincible)
        {
            healthBar.value -= 0.1f;
            StartCoroutine(Invincible());
        }
    }

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