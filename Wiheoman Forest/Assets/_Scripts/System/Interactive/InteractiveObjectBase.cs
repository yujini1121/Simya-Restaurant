using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     모든 상호작용 가능한 대상들의 공통되는 것을 통제합니다
/// </summary>
/// <remarks>
///     일단 해당 영역에 닿는다면, 플레이어의 상호작용 목록에 들어갑니다.
/// </remarks>
public abstract class InteractiveObjectBase : MonoBehaviour
{
    /// <summary>
    ///     상속하는 대상은 해당 함수를 구현해야 합니다.
    /// </summary>
    public abstract void DoInteractiveWithThis();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == PlayerController.instance.gameObject.name)
        {
            PlayerController.instance.AddInteractive(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == PlayerController.instance.gameObject.name)
        {
            PlayerController.instance.RemoveInteractive(this);
        }
    }
}
