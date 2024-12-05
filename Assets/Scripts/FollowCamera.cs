using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // ���� ĳ����
    public Vector3 offset = new Vector3(0, 5, -10); // ������ ������
    public float smoothSpeed = 0.125f; // ī�޶� �������� �ε巯�� ����

    void LateUpdate()
    {
        if (target == null) return;

        // ��ǥ ��ġ ���
        Vector3 desiredPosition = target.position + offset;

        // �ε巴�� ī�޶� �̵�
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // ī�޶� ��ġ ������Ʈ
        transform.position = smoothedPosition;

        // �׻� ĳ���͸� �ٶ󺸰� �ϱ� (�ʿ� �� �ּ� ó�� ����)
        transform.LookAt(target);
    }
}
