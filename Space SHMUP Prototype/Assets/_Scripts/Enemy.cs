using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;// �������� � �/�
    public float fireRate = 0.3f;// ������� ����� ����������
    public float health = 10;// ���������� ������
    public int score = 100;// ���� �� ����������� �������

    //������� �� ��������� ��������� 
    public float showDamageDuration = 0.1f;//������������ ������ ��������� � �������
    public float powerUpDropChance = 1f;//���� �������� �����
    [Header("Set Dynamically")]
    public Color[] originalColors;//����� ����� �������� ������������ ����� �������� ������ Enemy � ��� �������� ������
    public Material[] materials;//��� ��������� �������� ������� � ��� ��������
    public bool showingDamage = false;//�������� ���� true - ������ ������� ������ ������� � �������, ���� false - ���
    public float damageDoneTime;//����� ����������� ����������� ������
    public bool notifiedOfDestruction = false;// ����� ������������� ����� - ����� ���� �������� �����
    protected BoundsCheck bndCheck;// ������ �� ��������� BoundsCheck, ��� ��������� � ����� �������� �������
    //��� ��������: �����, ����������� ��� ����
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);// ���������� ���������� �������� �������  
        }
        set
        {
            this.transform.position = value;// �������������� ����������
        }
    }
    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        //�������� ������ �� ��������� �������� BoundsCheck ��� ��������� � ����� �������� �������

        //�������� ��������� � ���� ����� ������� � ��� ��������
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];//���������������� ����� ��� ������ ������������ ������ �������
        for(int i =0; i<materials.Length; i++)
        {
            /*
             * � ������� ����� ����������� ����� ���� ���������� � ��������� �� �������� ����� 
             * � ����� originalColors
             */
            originalColors[i] = materials[i].color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();// ���������� ������� ���� 
        if(showingDamage && Time.time> damageDoneTime)
        {
            //���� showingDamage = true; �
            //Time.time> damageDoneTime �� ����
            //���� ����� ������ �� ����� ����������� ������ ����������� ���������  - ���������� �����
            UnShowDamage();
        }
        if (bndCheck != null && bndCheck.offDown)
        {

            //��������, ��� ������� ����� �� ������ ������� - ����������
            Destroy(gameObject);
        }
    }
    public virtual void Move()
    {
        Vector3 temPos = pos; // �������� ������� ���������� � ���������� temPos
        temPos.y -= speed * Time.deltaTime;// ����������� ������ ����
        pos = temPos;//����������� �������
    }
    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;// �������� ������ �� ������� ������ ��� ���������� � ������
        switch(otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();//������ ��������� Projectile(���) �������� �������

                //���� ��������� ������� �� ��������� ������
                //�� �������� ��� �����������

                if(!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);//���������� ������
                    break;
                }

                //�������� ��������� �������
                ShowDamage();//���������� ���������
                //�������� ����������� ���� � ����� Main
                health -= Main.GetWeaponDefinion(p.type).damageOnHit;//������� ����
                if(health<=0)
                {
                    //�������� Main ��� ����������� �������
                    if(!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);//������� �����
                    }
                    notifiedOfDestruction = true;
                    //���� ����� ������ ��� 0 ���������� �����
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);//���������� ������
                break;
            case "Rocket"://���� ���������� � ������� 
                Rocket r = otherGO.GetComponent<Rocket>();//������ ��������� Rocket(���) �������� �������
                //���� ��������� ������� �� ��������� ������
                //�� �������� ��� �����������

                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);//���������� ������
                    break;
                }

                //�������� ��������� �������
                ShowDamage();//���������� ���������
                //�������� ����������� ���� ������ � ����� Main
                health -= Main.GetWeaponDefinion(r.type).damageOnHit;
                if (health <= 0)
                {
                    //�������� Main ��� ����������� �������
                    if (!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);//������� �����
                    }
                    notifiedOfDestruction = true;
                    //���� ����� ������ ��� 0 ���������� �����
                    Destroy(this.gameObject);
                }
                print("Boom");
                Destroy(otherGO);//���������� ������
                break;
            default:
                print("Enemy hit by non-Projectile " + otherGO.name);
                break;
        }
    }

    /// <summary>
    /// ����� ShowDamage - ���������� ������� �� ��������� 
    /// ������ �������������� ���� ���������� ������� � ������� ����
    /// </summary>
    void ShowDamage()
    {
        foreach(Material m in materials)
        {
            //� ����� ���������� ����� ���� ���������� � ������������� � ������� ����
            m.color = Color.red;
        }
        showingDamage = true;//���������� ��� ��������� ����������
        damageDoneTime = Time.time + showDamageDuration;//��������� ����� ��������� ������
    }

    /// <summary>
    /// ������� UnShowDamage - ���������� ����� ������� ���������
    /// ������ �������������� ���� ���������� ������� � �������� ����
    /// </summary>
    void UnShowDamage()
    {
        for(int i = 0; i<materials.Length; i++)
        {
            //� ����� ���������� ����� ���� ���������� � ������������� � �������� ����
            materials[i].color = originalColors[i];
        }
        showingDamage = false;//������������� ��� ����� ����������
    }
}
