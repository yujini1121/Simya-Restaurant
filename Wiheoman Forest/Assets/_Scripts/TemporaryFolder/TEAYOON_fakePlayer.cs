using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEAYOON_fakePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyBase.SetPlayer(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
