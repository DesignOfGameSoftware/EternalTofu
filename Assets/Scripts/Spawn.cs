using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public GameObject carNpc;
    public GameObject carNpc2;
    public GameObject positionNPC;
    public string     positionSpawn;

    private float spawnTime;
    private float cdSpawn;
    private int   typeNPC;
    private Quaternion Quaternion;

    private void Start()
    {
        spawnTime = Random.Range(0.2f, 0.3f);

        if(positionSpawn == "left")
        {
            Quaternion = new Quaternion(0, 180, 0, 1);
        }
        else
        {
            Quaternion = Quaternion.identity;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        cdSpawn = Coldown.add(cdSpawn);

        if (cdSpawn >= spawnTime)
        {
            spawnTime = Random.Range(0.7f, 2f);
            typeNPC   = Random.Range(0, 2);

            if (typeNPC == 1)
            {
                Instantiate(carNpc, positionNPC.transform.position, Quaternion);
            }
            else
            {
                Instantiate(carNpc2, positionNPC.transform.position, Quaternion);
            }

            cdSpawn = Coldown.reset();
        }
    }
}
