using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);

    [SerializeField] private Vector2 minCamLimit;
    [SerializeField] private Vector2 maxCamLimit;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        desiredPos.x = Mathf.Clamp(desiredPos.x, minCamLimit.x, maxCamLimit.x);
        desiredPos.y = Mathf.Clamp(desiredPos.y, minCamLimit.y, maxCamLimit.y);

        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * followSpeed);
    }
}
