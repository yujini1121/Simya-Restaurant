using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Drop : EnemyBase
{
    bool isDie = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IsDeadTest());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void DoDeathHandle()
    {
        if(!isDie)
        {
            isDie = true;
            gameObject.SetActive(false);
            DropItems();
        }
    }

    private IEnumerator IsDeadTest()
    {
        yield return new WaitForSeconds(3.0f);
        DoDeathHandle();
    }
}
