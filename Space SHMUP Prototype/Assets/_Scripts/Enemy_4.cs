using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Part - ���� ������� ������ ������ ������ ������ �������
/// </summary>
[System.Serializable]
public class Part
{
    //�������� ���� ��� ���� ������ ����������� � ����������
    public string name;//��� ���� �����
    public float health;//������� ��������� ���� ����� 
    public string[] protectedBy;//������ �����, �������� ���

    //��� ��� ��� ���������������� ������������� � Start()
    [HideInInspector]//�� ��������� ���������� ���� ��������� � ����������
    public GameObject go;//������� ������ ���� ����� 
    [HideInInspector]
    public Material mat;// �������� ��� ����������� �����������

    
}
/// <summary>
/// Enemy_4 ��������� �� ������� ��������, �������� ��������� �����  �� ������ 
/// � ������������ � ���. ���������� �� ����� ��������  ������  ��������� �����
/// � ���������� ��������  ���� ����� ���  �� ����������
/// </summary>
public class Enemy_4 : Enemy
{
    [Header("Set in Inspector: Enemy_4")]
    public Part[] parts;//����� ������ ������������ �������
    private Vector3 p0, p1;//��� ����� ��� ����������� 
    private float timeStart;//����� �������� ����� ������� 
    private float duration = 4;//����������������� ����������� 
    //�������� ������ �������� 
    public delegate void WeaponEnemyDelegate();
    //������� ���� ���� WeaponEnemyDelegate � ������ fireEnemy
    public WeaponEnemyDelegate fireEnemy;
    // Start is called before the first frame update
    void Start()
    {
        //��������� ������� ��� ������� � Main.SpawnEnemy()
        //������� ������� �� ��� ��������� �������� � p0, p1
        p0 = p1 = pos;
        InitMovement();

        //�������� � ��� ������� ������ � �������� ������ ����� � parts
        Transform t;
        foreach(Part prt in parts)
        {
            t = transform.Find(prt.name);//����� ������� ������ �� �����
            if(t!= null)//��������� ���� �� ����������
            {
                prt.go = t.gameObject;//�������� ������ �� ������ 
                prt.mat = prt.go.GetComponent<Renderer>().material;//������ ��������  Renderer.material
            }
        }

    }

    void InitMovement()
    {
        p0 = p1;//���������� p1 � p0
        //������� ����� ����� p1 �� ������
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        //�������� �����
        timeStart = Time.time;
    }
    // Update is called once per frame

    public override void Move()
    {
        //���� ����� �������������� ����� Enemy.Move()
        //� ��������� �������� ������������
        float u = (Time.time - timeStart) / duration;
        if(u >= 1)
        {
            InitMovement();
            fireEnemy();
            u = 0;
        }
        u = 1 - Mathf.Pow(1 - u, 2);//��������� ������� ����������
        pos = (1 - u) * p0 + u * p1;//������� �������� ������������
    }

    /// <summary>
    /// ��� ������� ��������� ����� ����� ������� � ������ parts
    /// �� ����� ���� �� ������ �� ������� ������ 
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    Part FindPart(string n)
    {
        foreach(Part prt in parts)
        {
            if(prt.name == n)
            {
                return (prt);
            }
        }
        return (null);
    }
    Part FindPart(GameObject go)
    {
        foreach(Part prt in parts)
        {
            if(prt.go == go)
            {
                return (prt);
            }
        }
        return (null);
    }

    /// <summary>
    /// ��� ������� ���������� true ���� ������ ����� ����������
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }
    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }
    bool Destroyed(Part prt)
    {
        if(prt == null)//���� ������ �� ����� �� ���� �������� 
        {
            return (true);//����� ���� ����������
        }
        //������� ��������� ���������  prt.health <= 0 - ���� ��� ������������� ��� �� ������� true, ���� ��� false
        return (prt.health <= 0);
    }
    /// <summary>
    /// ���������� � ������� ���� ������ ���� �����, � �� ���� �������
    /// </summary>
    /// <param name="m"></param>
    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }
    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch(other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                //���� ������� �� �� ��������� �� �������� ����
                if(!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }
                //�������� ��������� �������
                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if(prtHit == null)
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                //��������� �������� �� ��� ����� ������� 
                if(prtHit.protectedBy != null)
                {
                    foreach(string s in prtHit.protectedBy)
                    {
                        //���� ������ ���� �� ������ ��� �� ��������� 
                        if(!Destroyed(s))
                        {
                            //�� �������� ����������� ���� ����� 
                            Destroy(other);
                            return;
                        }
                    }
                }
                //��� ����� �� ��������, ������� �� �����������
                //�������� ���������� ����
                prtHit.health -= Main.GetWeaponDefinion(p.type).damageOnHit;
                //�������� ������ ��������� � ����� 
                ShowLocalizedDamage(prtHit.mat);
                if(prtHit.health<=0)
                {
                    //������ ���������� ����� ������� 
                    //�������������� ������������ �����
                    prtHit.go.SetActive(false);
                }
                //��������� ��� �� ��������������� �������� 
                bool allDestroyed = true;//������������ ��� ��������
                foreach(Part prt in parts)
                {
                    if(!Destroyed(prt))//���� �����-�� ����� ��� �������
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if(allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    //���������� ���� ������ 
                    Destroy(this.gameObject);
                }
                Destroy(other);//���������� ������
                break;
            case "Rocket":
                Rocket r = other.GetComponent<Rocket>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }
                //�������� ��������� �������
                GameObject _goHit = coll.contacts[0].thisCollider.gameObject;
                Part _prtHit = FindPart(_goHit);
                if (_prtHit == null)
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                //��������� �������� �� ��� ����� ������� 
                if (_prtHit.protectedBy != null)
                {
                    foreach (string s in _prtHit.protectedBy)
                    {
                        //���� ������ ���� �� ������ ��� �� ��������� 
                        if (!Destroyed(s))
                        {
                            //�� �������� ����������� ���� ����� 
                            Destroy(other);
                            return;
                        }
                    }
                }
                //��� ����� �� ��������, ������� �� �����������
                //�������� ���������� ����
                _prtHit.health -= Main.GetWeaponDefinion(r.type).damageOnHit;
                //�������� ������ ��������� � ����� 
                ShowLocalizedDamage(_prtHit.mat);
                if (_prtHit.health <= 0)
                {
                    //������ ���������� ����� ������� 
                    //�������������� ������������ �����
                    _prtHit.go.SetActive(false);
                }
                //��������� ��� �� ��������������� �������� 
                bool _allDestroyed = true;//������������ ��� ��������
                foreach (Part prt in parts)
                {
                    if (!Destroyed(prt))//���� �����-�� ����� ��� �������
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if (_allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    //���������� ���� ������ 
                    Destroy(this.gameObject);
                }
                Destroy(other);//���������� ������
                break;
        }
    }
}
