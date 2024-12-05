using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // 따라갈 캐릭터
    public Vector3 offset = new Vector3(0, 5, -10); // 고정된 오프셋
    public float smoothSpeed = 0.125f; // 카메라 움직임의 부드러움 정도

    void LateUpdate()
    {
        if (target == null) return;

        // 목표 위치 계산
        Vector3 desiredPosition = target.position + offset;

        // 부드럽게 카메라 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 카메라 위치 업데이트
        transform.position = smoothedPosition;

        // 항상 캐릭터를 바라보게 하기 (필요 시 주석 처리 가능)
        transform.LookAt(target);
    }
}
