using UnityEngine;
using System.Collections;

public class GridMap : MonoBehaviour
{

    static public GridMap Instance = null;


    // 是否显示场景信息
    public bool m_debug = false;

    // 场景的大小
    public int MapSizeX = 32;
    public int MapSizeZ = 32;

    // 一个二维数组用于保存场景信息
    public MapData[,] m_map;

    void Awake()
    {
        Instance = this;

        // 初始化场景信息
        this.BuildMap();
    }


    // 创建地图
    [ContextMenu("BuildMap")]
    public void BuildMap()
    {
        //创建二维数组
        m_map = new MapData[MapSizeX, MapSizeZ];

        for (int i = 0; i < MapSizeX; i++)
        {
            for (int k = 0; k < MapSizeZ; k++)
                m_map[i, k] = new MapData();
        }

       
        // 获得所有Tag为gridnode的节点
        GameObject[] nodes = (GameObject[])GameObject.FindGameObjectsWithTag("gridnode");

        foreach (GameObject nodeobj in nodes)
        {
            //获得节点
            GridNode node = nodeobj.GetComponent<GridNode>();

            Vector3 pos = nodeobj.transform.position;

            //如果节点的位置超出场景范围，则忽略
            if ((int)pos.x >= MapSizeX || (int)pos.z >= MapSizeZ)
                continue;

            //设置格子的属性
            m_map[(int)pos.x, (int)pos.z].fieldtype = node._mapData.fieldtype;

        }

    }


    //绘制场景信息
    void OnDrawGizmos()
    {
        if (!m_debug || m_map == null)
            return;

        // 线条的颜色
        Gizmos.color = Color.blue;

        // 绘制线条的高度
        float height = 0;

        // 绘制网格
        for (int i = 0; i < MapSizeX; i++)
        {
            Gizmos.DrawLine(new Vector3(i, height, 0), new Vector3(i, height, MapSizeZ));
        }
        for (int k = 0; k < MapSizeZ; k++)
        {
            Gizmos.DrawLine(new Vector3(0, height, k), new Vector3(MapSizeX, height, k));
        }

        // 改为红色
        Gizmos.color = Color.red;
        
        for (int i = 0; i < MapSizeX; i++)
        {
            for (int k = 0; k < MapSizeZ; k++)
            {
                //在不能放置防守区域的方格内绘制红色的方块
                if (m_map[i,k].fieldtype == MapData.FieldTypeID.CanNotStand)
                {
                    Gizmos.color = new Color(1, 0, 0, 0.5f);

                    Gizmos.DrawCube(new Vector3(i + 0.5f, height, k + 0.5f), new Vector3(1, height + 0.1f, 1));

                }
            }
        }

    }

}
