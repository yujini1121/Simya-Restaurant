using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     여러가지 요리 완성품을 정의하며, 아이템을 소지하도록 만듭니다.
/// </summary>
public class FoodHolder : MonoBehaviour
{
    // 여러 음식들을 가지고 있습니다.
    // 플레이어 컴포넌트에 부착되어 있습니다.
    FoodComponent currentHoldingFood;

    public void MakeFood(FoodComponent food)
    {
        currentHoldingFood = food;
    }

    public void SellDish()
    {
#warning TODO 플레이어 소지금에 접근하는 코드 작성바랍니다
        // 플레이어의 소지금에 접근하여 currentHoldingFood.price 값만큼 추가하면 됩니다.

        currentHoldingFood = null;
    }

    public FoodComponent TEST_GetCheeseBurger()
    {
        return new FoodComponent("CheeseBurger", 2_800_000);
    }
    public FoodComponent TEST_GetNoodle()
    {
        return new FoodComponent("수박키위냉면", 800);
    }
}
