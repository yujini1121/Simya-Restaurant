using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveEffectController : MonoBehaviour
{
    public Material shockwaveMaterial;
    public Transform shockwaveOrigin;
    public float maxRadius = 1.0f;
    public float duration = 1.0f;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float elapsed = Time.time - startTime;
        float progress = elapsed / duration;

        if (progress > 1.0f)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 screenPos = Camera.main.WorldToViewportPoint(shockwaveOrigin.position);
        shockwaveMaterial.SetVector("_WaveCenter", new Vector4(screenPos.x, screenPos.y, 0, 0));
        shockwaveMaterial.SetFloat("_WaveRadius", progress * maxRadius);
    }
}
