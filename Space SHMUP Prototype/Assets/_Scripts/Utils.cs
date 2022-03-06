using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    //����������  ������ ���� ���������� � ������ ������� ������� 
    //� ��� �������� ��������
    // Start is called before the first frame update
    /// <summary>
    /// ����������� ����� GetAllMaterials ��� ���������� ����� ���������� �� �������� �������
    /// </summary>
    /// <param name="go">��������� ������ �� �������� ����� ������� ����� ����������</param>
    /// <returns>���������� ����� ���������� ������ �������� ������� </returns>
    static public Material[] GetAllMaterials(GameObject go)
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();//��������� ����� ������ �������� ������� � ���� �������� ��������
        //� ��������� ����� ���������� � ������ Renderer
        List<Material> mats = new List<Material>();//������� ������ � ����������� 
        foreach(Renderer rend in rends)
        {
            /*
             * ���� ���� ��������� ����� ���� ����������� Renderer � ������ rends
             * � ��������� �������� ���� ������� �� ��� 
             * ����� ��������� �������� ����������� � ������� mats
             * */
            mats.Add(rend.material);
        }
        return (mats.ToArray());// ������ mats ������������� � ����� � ���������� ����������� ����
    }


}
