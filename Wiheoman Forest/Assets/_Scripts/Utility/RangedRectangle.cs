using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangedRectangle
{
    public Vector3 leftDown;
    public Vector3 rightUp;

    public Vector3 GetRandomPoint()
    {
        float x = Random.Range(leftDown.x, rightUp.x);
        float y = Random.Range(leftDown.y, rightUp.y);

        return new Vector3(x, y, 0);
    }
}
