using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector2 rotMinMax = new Vector2(15, 25);//��� �������� ������������ � ������������� �������� ��� ������ Random.Range
    public Vector2 driftMinMax = new Vector2(.25f, 2); //��� �������� ������������ � ������������� �������� ��� ������ Random.Range
    public float lifeTime = 6f;//����� � �������� ������������� PowerUp
    public float fadeTime = 4f;//����� ������������ PowerUp

    [Header("Set Dynamically")]
    public WeaponType type;//��� ������
    public GameObject cube;//������ �� ��������� ���
    public TextMesh letter;// ������ �� TextMesh
    public Vector3 rotPerSecond;//�������� ��������
    public float birthTime;// ����� ��������
    private Rigidbody rigid;//������ �� ��������� Rigidbody
    private BoundsCheck bndCheck;//������ �� ��������� BoundsCheck
    private Renderer cubeRend;
    // Start is called before the first frame update
    void Awake()
    {
        //�������� ������ �� ���
        cube = transform.Find("Cube").gameObject;
        //�������� ������ �� TextMesh
        letter = GetComponent<TextMesh>();
        //�������� ������ �� Rigidbody
        rigid = GetComponent<Rigidbody>();
        //�������� ������ �� BoundsCheck
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        //������� ��������� �������� 
        Vector3 vel = Random.onUnitSphere;//�������� ��������� �������� XYZ
        /*
         * Random.onUnitSphere - ���������� ������, ����������� �� ��������� �����,
         * ����������� �� ����������� �����  � �������� 1 � ������� � ����� ������ ��������
         */
        vel.z = 0;//���������� vel �� ��������� XY
        vel.Normalize();//������������ ������������� �����  Vector3 ������ 1 
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);//����� ��������� �������� 
        rigid.velocity = vel;

        //���������� ���� �������� ����� �������� ������� ������ R[0,0,0]
        transform.rotation = Quaternion.identity;// ���������� �������� 
        //������� ��������� �������� �������� ����
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));
        birthTime = Time.time;//����� �������� 
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);// ������� ���� � ������ �����

        //����� ����������� ���� PowerUp � �������� �������
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        //� ������� lifeTime ������ �������� u <=0.
        //���� ������� ����� ����� ������ �� birthTime + lifeTime u>=0
        //���� ������� ����� ����� ������ �� (birthTime + lifeTime)+fadeTime u>=1
        if(u>=1)
        {
            Destroy(this.gameObject);
            return;

        }
        if(u>0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;

            //����� ���� ������ ����������� �� ���������
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }
        if(!bndCheck.isOnScreen)//���� ����� �� ������� ����������
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ������� ��� ��������� ���� ������
    /// ��� ������
    /// </summary>
    /// <param name="wt"> ��� �� ������� ����� �������� �����</param>
    public void SetType(WeaponType wt)
    {
        //�������� weaponDefinion �� Main
        WeaponDefinion def = Main.GetWeaponDefinion(wt);
        //���������� ���� ��������� ���� 
        cubeRend.material.color = def.color;
        //letter.color = def.color;
        letter.text = def.letter;
        type = wt;//���������� ����������� ��� ������ 
    }

    /// <summary>
    /// ��� ������� ���������� ������� ����, ����� ����� ��������� �����
    /// </summary>
    /// <param name="target"></param>
    public void AbsorbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
