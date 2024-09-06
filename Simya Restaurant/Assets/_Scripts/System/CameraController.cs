using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float followSpeed = 0.2f;

    [SerializeField] private Vector3 adjustCamPos = Vector3.zero; // 카메라 위치 조정

    [SerializeField] private Vector2 minCamLimit;
    [SerializeField] private Vector2 maxCamLimit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        Vector3 pos = Vector3.Lerp(transform.position, target.position, followSpeed);
        transform.position = new Vector3(Mathf.Clamp(pos.x, minCamLimit.x, maxCamLimit.x) + adjustCamPos.x,
            Mathf.Clamp(pos.y, minCamLimit.y, maxCamLimit.y) + adjustCamPos.y, -10f + adjustCamPos.z);
    }
}
