using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;// ������ ��������
    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;// ����� �������� Enemy
    public float enemySpawnPerSecond = 0.5f;// �������� ��������� �������� �� ������� �������
    public float enemyDefaultPadding = 1.5f;
    //������ ��� ����������������.
    private BoundsCheck bndCheck;
    // Start is called before the first frame update
    void Awake()
    {
        S = this;
        //�������� �  bndCheck ������ �� ��������� BoundsCheck
        bndCheck = GetComponent<BoundsCheck>();
        //������� SpawnEnemy
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    public void SpawnEnemy()
    {
        // ������� ��������� ������ Enemy ��� �������� 
        int ndx = Random.Range(0, prefabEnemies.Length);//������� ��������� ������ 
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);// ������� ������ �� ������ prefabEnemies ��� �������� ndx
        //���������� ��������� ������� ��� ������� � ��������� ������� �
        float enemyPadding = enemyDefaultPadding;// �������� enemyPadding ��������� ������ �� ������
        if (go.GetComponent<BoundsCheck>()!= null)//��������� ���� �� � ������� ��������� BoundsCheck 
        {
            //���� ���� �������� � enemyPadding  - radius ����������� �������� ����������  
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        Vector3 pos = Vector3.zero;// ������� ���������� pos ���� Vector3 � ��������� ��������� ���������� (0,0,0)
        float xMin = -bndCheck.camWidth + enemyPadding;//���������� ����������� �������� ������� �� ��� � ��� ����� ��������� ������
        float xMax = bndCheck.camWidth - enemyPadding;//���������� ������������ �������� ������� �� ��� � ��� ����� ��������� ������
        pos.x = Random.Range(xMin, xMax);//������� ��������� ���������� ����� xMin � xMax ������������
        pos.y = bndCheck.camHeight + enemyPadding;// ���������� ������ �� ������� ����� ��������� ������
        go.transform.position = pos;//����������� ������ �� ������� ���������� pos
        //����� ������� SpawnEnemy()
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    public void DelayedRestart(float delay)
    {
        //  ������� ����� Restart() ����� delay ������
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
