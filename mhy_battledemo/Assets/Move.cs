using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody RigidThis;
    private Transform TransformThis;
    private Animator AnimThis;
    public float MoveSpeed = 2f;
    public float RotationSpeed = 0.001f;
    public float BackSpeedRatio = 0.8f;
    AnimatorStateInfo Info;

    private int BuffTime = 0;
    private bool BuffON = false;
    private GameObject BuffParticle;
    private GameObject MCamera;
    private GameObject SCamera;

    public float AnimStopTime = 0.1f;
    private float AnimStopTimer = 0.0f;
    private bool IsAnimStop = false;

    void Start()
    {
        TransformThis = this.GetComponent<Transform>();
        RigidThis = this.GetComponent<Rigidbody>();
        AnimThis = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //��ǰ����״̬
        Info = AnimThis.GetCurrentAnimatorStateInfo(0);
        //�ݷ�������
        float verti = Input.GetAxis("Vertical");
        //�᷽������
        float hori = Input.GetAxis("Horizontal");

        BuffParticle = transform.Find("BuffParticle").gameObject;
        // MCamera = transform.Find("Main Camera").gameObject;
        // SCamera = transform.Find("Sub Camera").gameObject;


        //����1BUFF��ʱ
        SkillBuff();

        //�ƶ�
        if ((!Info.IsName("DIE") && !Info.IsName("SKILL_01") && !Info.IsName("SKILL_02") && !Info.IsName("MELEE_01") && !Info.IsName("MELEE_02") )||(Info.IsName("MELEE_01") && CanCombo(Info, 0.5f, 1.0f))||(Info.IsName("MELEE_02") && CanCombo(Info, 0.5f, 1.0f))||(Info.IsName("MELEE_04") && CanCombo(Info, 0.6f, 1.0f)))
        {
            //if (!Input.GetMouseButton(1))
            {
                if (verti > 0.1f) //��ǰ�ƶ�
                {
                    TransformThis.Translate(new Vector3(0, 0, 1) * Time.deltaTime * MoveSpeed * verti);
                    TransformThis.Translate(new Vector3(1, 0, 0) * Time.deltaTime * MoveSpeed * hori * BackSpeedRatio);  //ǰ��ʱ�����ƶ��ٶȱ���
                }
                else if (verti < -0.1f) //���ƶ�
                {
                    TransformThis.Translate(new Vector3(0, 0, 1) * Time.deltaTime * MoveSpeed * verti * BackSpeedRatio);//���ƶ��ٶȱ���
                    TransformThis.Translate(new Vector3(1, 0, 0) * Time.deltaTime * MoveSpeed * hori * BackSpeedRatio);  //���ƶ�ʱ�����ƶ��ٶȱ���
                }
                else //ǰ�����ƶ�
                {
                    TransformThis.Translate(new Vector3(1, 0, 0) * Time.deltaTime * MoveSpeed * hori * BackSpeedRatio); //�������ƶ��ٶȱ���
                }
            }
            // else
            // {
            //     if (verti > 0.1f) //��ǰ�ƶ�
            //     {
            //         TransformThis.Translate(new Vector3(0, 0, 1) * Time.deltaTime * MoveSpeed * verti);
            //     }
            //     else
            //     {
            //         TransformThis.Translate(new Vector3(0, 0, 1) * Time.deltaTime * MoveSpeed * verti * BackSpeedRatio);//���ƶ��ٶȱ���
            //     }
             
            //     TransformThis.Rotate(new Vector3(0, 0.4f, 0) * RotationSpeed * hori);//�����ǿ���޷����ƣ�Ϊʲô��
            //  }

            //右键不再控制镜头转向，故注掉
        }

       //�ƶ�״̬�ı�
       if ((verti > 0.1f) || (verti < -0.1f) || (hori > 0.1f) || (hori < -0.1f))
       {
            AnimThis.SetBool("ToRun", true); 
       }
       else
       {
            AnimThis.SetBool("ToRun", false);
       }
       
  

        //�������ʱ - RUN��ʹ��RUSHATK�����������

        if (Input.GetMouseButtonDown(0))
        {
            if ((Info.IsName("IDLE")) || Info.IsName("RUN"))
            {
                AnimThis.SetBool("ToMelee_01", true);
                //Attack(2.0f);
            }
            if ((Info.IsName("RUN") && BuffON )) 
            {
                AnimThis.SetBool("ToMelee_01", true);
                AnimThis.SetBool("ToRA", true);
                //Attack(4.0f);
            }
            if (Info.IsName("MELEE_01")&& CanCombo(Info, 0.3f, 0.7f))
            {
                AnimThis.SetBool("ToMelee_02", true);
                AnimThis.SetBool("ToMelee_01", false);
                //Attack(3.0f);
            }
            if (Info.IsName("MELEE_02") && CanCombo(Info, 0.35f, 0.75f))
            {
                AnimThis.SetBool("ToMelee_03", true);
                AnimThis.SetBool("ToMelee_02", false);
                //Attack(4.0f);
            }
            if (Info.IsName("MELEE_03") && CanCombo(Info, 0.45f, 0.75f) && BuffON) 
            {
                AnimThis.SetBool("ToMelee_04", true);
                AnimThis.SetBool("ToMelee_03", false);
                //Attack(6.0f);
            }

        }
        else
        {
            AnimThis.SetBool("ToMelee_01", false);
            AnimThis.SetBool("ToMelee_02", false);
            AnimThis.SetBool("ToMelee_03", false);
            AnimThis.SetBool("ToMelee_04", false);
            AnimThis.SetBool("ToRA", false);
        }

        //����Q�뼼��E
        if (!Info.IsName("DIE"))   //(Info.IsName("IDLE") || Info.IsName("RUN") )
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                AnimThis.SetBool("ToSkill_01", true);
                BuffTime = 4500;
            }
            else
            {
                AnimThis.SetBool("ToSkill_01", false);
            }
            if (Input.GetKeyDown(KeyCode.E)||Input.GetMouseButton(1))
            {
                AnimThis.SetBool("ToSkill_02", true);
                //Attack(3.0f);
            }
            else
            {
                AnimThis.SetBool("ToSkill_02", false);
            }
        }
        

        //��ɱ�븴��
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!Info.IsName("DIE"))
            {
                AnimThis.SetTrigger("ToDieTrigger");
                AnimThis.SetBool("ToDie", true);
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Z) && Info.IsName("DIE"))

            AnimThis.SetBool("ToDie", false);
        }

        //��֡Ч��
        if(IsAnimStop)
        {
            AnimThis.speed = 0;
            AnimStopTimer += Time.deltaTime;
            if(AnimStopTimer >= AnimStopTime)
            {
                IsAnimStop = false;
                AnimStopTimer = 0;
            }
        }
        else
        {
            AnimThis.speed = 1;
        }
    }
    
    //��������
    private bool CanCombo(AnimatorStateInfo info , float start, float end)
    {
        return (info.normalizedTime >= start) && (info.normalizedTime <= end);
    }

    //BUFF����
     private void SkillBuff()
     {
        if (BuffTime > 0)
        {
            BuffON = true;
            BuffTime--;
            //Debug.Log(BuffTime);
            BuffParticle.SetActive(true);

            // MCamera.transform.GetComponent<Camera>().enabled = false;
            // SCamera.transform.GetComponent<Camera>().enabled = true;
        }
        else
        {
            BuffON = false;
            AnimThis.SetBool("ToMelee_04", false);
            AnimThis.SetBool("ToRA", false);
            BuffParticle.SetActive(false);

            // MCamera.transform.GetComponent<Camera>().enabled = true;
            // SCamera.transform.GetComponent<Camera>().enabled = false;
        }
     }
   
    private void Attack(float skillDis)
    {
        float skillDistance = skillDis;

        if ((Info.IsName("IDLE")) || Info.IsName("RUN")) //MELEE_01
        {
            skillDistance = 2.0f;
        }
        if ((Info.IsName("RUN") && BuffON)) //RA
        {
            skillDistance = 4.0f;
        }
        if (Info.IsName("MELEE_01") && CanCombo(Info, 0.3f, 0.7f))  //MELEE_02
        {
            skillDistance = 3.0f;
        }
        if (Info.IsName("MELEE_02") && CanCombo(Info, 0.35f, 0.75f))    //MELEE_03
        {
            skillDistance = 4.0f;
        }
        if (Info.IsName("MELEE_03") && CanCombo(Info, 0.5f, 0.8f) && BuffON)    //MELEE_04
        {
            skillDistance = 6.0f;
        }

        GameObject[] enemy = GameObject.FindGameObjectsWithTag("enemy"); //���й���
        List<GameObject> templist = new List<GameObject>();
        
        //���Ϲ�������ɸѡ
        for (int i = 0; i < enemy.Length; i++)
        {
            float dis = Vector3.Distance(transform.position, enemy[i].transform.position);
            float ang = Vector3.Angle(transform.forward, enemy[i].transform.position - transform.position);
         
            if (dis <= skillDistance && ang <= 60)
            {
                templist.Add(enemy[i]);
            }

        }

        foreach (var objects in templist)
        {
            if (objects.GetComponent<Rigidbody>() != null)
            {
                objects.GetComponent<Rigidbody>().AddExplosionForce(250, transform.position, 0);
                objects.GetComponent<Enemy>().Hitted();
                IsAnimStop = true;
            }
        }
    }
}
