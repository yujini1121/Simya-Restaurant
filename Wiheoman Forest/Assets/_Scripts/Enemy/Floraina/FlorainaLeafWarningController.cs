using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlorainaLeafWarningController : MonoBehaviour
{
    public float remainTime
    {
        get => remainTimeInspector;
    }

    [SerializeField] private float remainTimeInspector; // 인스펙터에서 입력하는 값

    private void Start()
    {
        MeshRenderer myMeshRenderer = GetComponent<MeshRenderer>();

        Color myColor = Color.red; // 반투명 빨강을 만들어야 하지만, 랜더링 모드를 바꿔야 했고, 랜더링 모드를 바꿀 수 없었습니다.
        
        myMeshRenderer.material.color = myColor;
        myMeshRenderer.enabled = true;
        Destroy(gameObject, remainTime);
    }
}
