using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     코드 재활용성을 극도로 높이는 정적 함수들로 이루어진 클래스입니다.
/// </summary>
public class UtilityFunctions : MonoBehaviour
{
    /// <summary>
    ///     베지어 곡선을 따라 대상이 움직이는 함수
    /// </summary>
    /// <param name="moveObject">움직일 대상입니다.</param>
    /// <returns></returns>
    /// <remarks>참조 링크 : https://leekangw.github.io/posts/49/</remarks>
    static public IEnumerator MoveOnBezierCurve(Vector3 start, Vector3 end, Vector3 center, GameObject moveObject, float duration)
    {
        float time = 0;

        while (time < duration)
        {
            // 부동소수점 오차도 문제 때문에 time += Time.deltaTime / timeForBerryMove 로직을 쓰지 않음
            float lerpValue = time / duration;
            Vector2 pointStartToCenter = Vector3.Lerp(start, center, lerpValue);
            Vector2 pointCenterToEnd = Vector3.Lerp(center, end, lerpValue);
            moveObject.transform.position = Vector3.Lerp(pointStartToCenter, pointCenterToEnd, lerpValue);

            time += Time.deltaTime;
            yield return null;
        }

        moveObject.transform.position = end; // 베리가 도착 지점에 정확히 위치함을 보장한다.
    }

    /// <summary>
    ///     대기 시간 후 호출할 함수의 일련을 준비합니다.
    /// </summary>
    /// <param name="actionSequences">
    ///     대기 시간과 대기시간 후 실행할 무명 메서드의 튜플입니다.
    /// </param>
    /// <returns>
    ///     코루틴을 돌려줍니다.
    /// </returns>
    static public IEnumerator RunAfterDelay(params (float waitingTime, System.Action nextAction)[] actionSequences)
    {
        // for문을 따로 떼어둔 것은, 호출 시점에서 오류를 미리 잡아두는 것이 가장 좋기 때문입니다.
        for (int index = 0; index < actionSequences.Length; ++index)
        {
            Debug.Assert(
                actionSequences[index].waitingTime >= 0.0f,
                $"오류_PlayerController.RunAfterDelay : {index + 1}번째 시퀀스의 waitingTime이 {actionSequences[index].waitingTime}이며 0보다 작습니다."
                );
            Debug.Assert(
                actionSequences[index].nextAction != null,
                $"오류_PlayerController.RunAfterDelay : {index + 1}번째 시퀀스의 nextAction이 널 값입니다.");
        }

        for (int index = 0; index < actionSequences.Length; ++index)
        {
            yield return new WaitForSeconds(actionSequences[index].waitingTime);
            actionSequences[index].nextAction();
        }
    }

    /// <summary>
    ///     대기 시간 후 호출할 함수를 준비합니다.
    /// </summary>
    /// <param name="waitingTime">
    ///     대기할 시간입니다.
    /// </param>
    /// <param name="nextAction">
    ///     waitingTime 뒤에 실행할 코드입니다. 무명 메서드를 넣을 수 있고, 혹은 입력과 출력이 void인 함수명을 넣을 수 있습니다.
    /// </param>
    /// <returns>
    ///     코루틴을 돌려줍니다.
    /// </returns>
    static public IEnumerator RunAfterDelay(float waitingTime, System.Action nextAction)
    {
        return RunAfterDelay((waitingTime, nextAction));
    }

    /// <summary>
    ///     원을 그리는 함수입니다. OnDrawGizmos 함수의 정의에 해당 함수를 호출하는게 좋습니다.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="segments"></param>
    static public void DrawGizmoCircle(Vector3 center, float radius, int segments)
    {
        float angle = 2 * Mathf.PI / segments;

        Vector3 prevPoint = center + new Vector3(radius, 0, 0);
        for (int i = 1; i <= segments; i++)
        {
            float x = Mathf.Cos(angle * i) * radius;
            float y = Mathf.Sin(angle * i) * radius;
            Vector3 nextPoint = center + new Vector3(x, y, 0);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }

    static public int GetRandom(params float[] weights)
    {
        if (weights.Length == 0)
        {
            return 0;
        }

        float m_sum = 0.0f;
        for (int index = 0; index < weights.Length; ++index)
        {
            m_sum += weights[index];
        }

        float probability = Random.value * m_sum;
        for (int index = 0; index < weights.Length - 1; ++index)
        {
            if (probability < weights[index])
            {
                return index;
            }
            probability -= weights[index];
        }
        return weights.Length - 1;
    }

    /// <summary>
    ///     느린 정렬 알고리즘입니다. 배열의 원소의 갯수가 100개 미만이면 적당합니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="original"></param>
    /// <param name="isRightFirst"></param>
    /// <returns></returns>
    static public void Sort<T>(ref T[] targetArray, System.Func<T, T, bool> isRightFirst)
    {
        for (int index1 = 0; index1 < targetArray.Length; ++index1)
        {
            for (int index2 = index1 + 1; index2 < targetArray.Length; ++index2)
            {
                if (isRightFirst(targetArray[index1], targetArray[index2]))
                {
                    T temp = targetArray[index1];
                    targetArray[index1] = targetArray[index2];
                    targetArray[index2] = temp;
                }
            }
        }
    }

    static public void SetSize(Transform transform, float sizeX, float sizeY, float offsetY)
    {
        transform.localScale = new Vector3(sizeX, sizeY, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, sizeY / 2 + offsetY + 0.5f, transform.position.z);
    }

    static public void Log(object callerInstance, object message)
    {
        Debug.Log($">> {callerInstance.GetType().Name} : {message.ToString()}");
    }

    static public Vector3 GetMiddleForBezierCurve(Vector3 left, Vector3 right, float top)
    {
        Vector3 result = new Vector3();
        result = Vector3.Lerp(left, right, 0.5f);
        result.y = top;
        return result;
    }    
}
