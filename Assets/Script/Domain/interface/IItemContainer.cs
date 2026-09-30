using UnityEngine;
// 칸 번호로 아이템을 넣고 꺼낼 수 있는 창고
public interface IItemContainer 
{
   Item GetItem(int index); //그 칸에 뭐야 있냐
   bool CanPlace(int index, Item item); // 그 칸에 놓아도 되냐
   void SetItem(int index, Item item); // 그 칸에 두기

}
