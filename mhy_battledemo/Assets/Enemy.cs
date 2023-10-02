using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float hitStayTimer = 0;
    float hitStayTime = 0.45f;
    bool hitted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(hitted)
        {
            {
                this.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }

            hitStayTimer += Time.deltaTime;
            if(hitStayTimer >= hitStayTime)
            {
                hitStayTimer = 0;
                hitted = false;
            }
        }
        else
        {
            this.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    public void Hitted()
    {
        hitted = true;
    }
}
