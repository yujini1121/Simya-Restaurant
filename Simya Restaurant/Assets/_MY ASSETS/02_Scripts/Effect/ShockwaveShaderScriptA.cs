using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveShaderA : MonoBehaviour
{
    public Material shockwaveMaterial;
    public float waveDuration = 1.0f;
    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float radius = Mathf.Lerp(0, 5, elapsedTime / waveDuration); // �ݰ� ����
        shockwaveMaterial.SetFloat("_ShockwaveRadius", radius);

        if (elapsedTime >= waveDuration)
        {
            Destroy(gameObject); // ���� �ð��� ������ ȿ�� ����
        }
    }
}
