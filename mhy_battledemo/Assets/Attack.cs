using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float skillDis = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("enemy"); //所有怪物
        List<GameObject> templist = new List<GameObject>();

        //符合攻击条件筛选
        for (int i = 0;i<enemy.Length;i++)
        {
            float dis = Vector3.Distance(transform.position, enemy[i].transform.position);
            float ang = Vector3.Angle(transform.forward, enemy[i].transform.position - transform.position);

            if (dis<skillDis && ang < 60)
            {
                templist.Add(enemy[i]);
            }
            
        }

        foreach (var objects in templist)
        {
            if (objects.GetComponent<Rigidbody>()!=null)
            {
                //objects.GetComponent<Rigidbody>().freezeRotation = true;
                objects.GetComponent<Rigidbody>().AddExplosionForce(20,transform.position,5);
            }
        }



    }
}
