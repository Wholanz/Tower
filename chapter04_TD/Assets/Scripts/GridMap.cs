using UnityEngine;
using System.Collections;

public class GridMap : MonoBehaviour
{

    static public GridMap Instance = null;


    // �Ƿ���ʾ������Ϣ
    public bool m_debug = false;

    // �����Ĵ�С
    public int MapSizeX = 32;
    public int MapSizeZ = 32;

    // һ����ά�������ڱ��泡����Ϣ
    public MapData[,] m_map;

    void Awake()
    {
        Instance = this;

        // ��ʼ��������Ϣ
        this.BuildMap();
    }


    // ������ͼ
    [ContextMenu("BuildMap")]
    public void BuildMap()
    {
        //������ά����
        m_map = new MapData[MapSizeX, MapSizeZ];

        for (int i = 0; i < MapSizeX; i++)
        {
            for (int k = 0; k < MapSizeZ; k++)
                m_map[i, k] = new MapData();
        }

       
        // �������TagΪgridnode�Ľڵ�
        GameObject[] nodes = (GameObject[])GameObject.FindGameObjectsWithTag("gridnode");

        foreach (GameObject nodeobj in nodes)
        {
            //��ýڵ�
            GridNode node = nodeobj.GetComponent<GridNode>();

            Vector3 pos = nodeobj.transform.position;

            //����ڵ��λ�ó���������Χ�������
            if ((int)pos.x >= MapSizeX || (int)pos.z >= MapSizeZ)
                continue;

            //���ø��ӵ�����
            m_map[(int)pos.x, (int)pos.z].fieldtype = node._mapData.fieldtype;

        }

    }


    //���Ƴ�����Ϣ
    void OnDrawGizmos()
    {
        if (!m_debug || m_map == null)
            return;

        // ��������ɫ
        Gizmos.color = Color.blue;

        // ���������ĸ߶�
        float height = 0;

        // ��������
        for (int i = 0; i < MapSizeX; i++)
        {
            Gizmos.DrawLine(new Vector3(i, height, 0), new Vector3(i, height, MapSizeZ));
        }
        for (int k = 0; k < MapSizeZ; k++)
        {
            Gizmos.DrawLine(new Vector3(0, height, k), new Vector3(MapSizeX, height, k));
        }

        // ��Ϊ��ɫ
        Gizmos.color = Color.red;
        
        for (int i = 0; i < MapSizeX; i++)
        {
            for (int k = 0; k < MapSizeZ; k++)
            {
                //�ڲ��ܷ��÷�������ķ����ڻ��ƺ�ɫ�ķ���
                if (m_map[i,k].fieldtype == MapData.FieldTypeID.CanNotStand)
                {
                    Gizmos.color = new Color(1, 0, 0, 0.5f);

                    Gizmos.DrawCube(new Vector3(i + 0.5f, height, k + 0.5f), new Vector3(1, height + 0.1f, 1));

                }
            }
        }

    }

}
