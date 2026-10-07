using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target; // 플레이어 드래그
    void LateUpdate()
    {
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z // z는 고정
        );
    }
}
