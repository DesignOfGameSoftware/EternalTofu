using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jeep : MonoBehaviour {

    private Transform mTransform;
    public float speed;
    public Transform target;

    // Use this for initialization
    void Start () {
        mTransform = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        mTransform.Translate(new Vector3(0, 0, speed) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            Destroy(gameObject);
        }
    }
}
