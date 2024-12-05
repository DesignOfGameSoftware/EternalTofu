using UnityEngine;

public class ColliderAdder : MonoBehaviour
{
    void Start()
    {
        // 씬에 있는 모든 3D 오브젝트를 찾아 콜라이더를 추가
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Rigidbody가 없고, Collider가 없는 오브젝트에만 BoxCollider 추가
            if (obj.GetComponent<Rigidbody>() == null && obj.GetComponent<Collider>() == null)
            {
                obj.AddComponent<BoxCollider>();  // 예시로 BoxCollider 추가
            }
        }
    }
}
