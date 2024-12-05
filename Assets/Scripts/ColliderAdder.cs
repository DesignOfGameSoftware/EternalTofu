using UnityEngine;

public class ColliderAdder : MonoBehaviour
{
    void Start()
    {
        // ���� �ִ� ��� 3D ������Ʈ�� ã�� �ݶ��̴��� �߰�
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Rigidbody�� ����, Collider�� ���� ������Ʈ���� BoxCollider �߰�
            if (obj.GetComponent<Rigidbody>() == null && obj.GetComponent<Collider>() == null)
            {
                obj.AddComponent<BoxCollider>();  // ���÷� BoxCollider �߰�
            }
        }
    }
}
