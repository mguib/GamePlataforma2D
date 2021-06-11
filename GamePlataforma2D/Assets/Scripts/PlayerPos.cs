using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    public static PlayerPos instace;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        instace = this;


        player = GameObject.FindGameObjectWithTag("Player").transform;

        if(player != null)
        {
            CheckPoint();
        }
        
    }

    public void CheckPoint()
    {
        Vector3 playerpos = transform.position;
        playerpos.z = 0f;

        player.position = playerpos;
    }

}
