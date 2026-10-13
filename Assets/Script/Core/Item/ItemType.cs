using UnityEngine;

public enum ItemType
{
    Resource,   // 자원(돌, 나무 등) - 장착 불가
    Armor,      // 방어구 - 슬롯에 장착 가능
    ConSumble   //소비 아이템(포션 등) - 우클릭으로 사용   
}