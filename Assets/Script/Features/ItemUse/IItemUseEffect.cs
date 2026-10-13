using UnityEngine;

//모든 "사용 효과"가 지켜야 하는 공통 규칙
//Presenter는 이 인터페이스만 알고 실제 효과는 모름
public interface IItemUseEffect 
{
    //효과 사용 시도
    // true = 사용 성공 -> Presenter가 아이템 개수 -1
    // flase = 사용 실패 -> 아이템 소모 안함
    bool TryUse(ItemUseContext context);
}
