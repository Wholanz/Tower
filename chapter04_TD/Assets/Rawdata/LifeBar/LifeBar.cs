using UnityEngine;
using System.Collections;

public class LifeBar : MonoBehaviour 
{
    // 当前生命
    public float m_currentLife = 1.0f;

    // 最大生命
    public float m_maxLife = 1.0f;
   
    internal Transform m_transform;

    // 横向缩放生命条
    float m_hscale = 1.0f;

    // 纵向缩放生命条
    float m_vscale = 1.0f;

    // 多边形模型组件
    Mesh m_mesh;

    Transform m_cameraTransform;

    //一个2维数组，用于保存UV 0==左下角, 1==右下角 , 2==左上角, 3==右上角
    Vector2[] m_Uvs;

    // 初始化
    public void Ini(float currentlife, float maxlife ,float hscale ,float vscale)
    {
       
        m_transform = this.transform;
        m_cameraTransform = Camera.main.transform;

        m_hscale = hscale;
        m_vscale = vscale;
        m_transform.localScale = new Vector3(hscale, vscale, 1.0f);

        // 获得模型组件
        m_mesh = (Mesh)this.GetComponent<MeshFilter>().mesh;

        // 获得模型的顶点
        Vector3[] vertices = m_mesh.vertices;

        // 获得所有的UV
        m_Uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            m_Uvs[i] = m_mesh.uv[i];
        }

        // 更新生命条状态
        UpdateLife(currentlife, maxlife);
       
    }

    // 移动生命条模型的UV
    void Pad( float value )
    {
        // 0==左下角, 1==右下角 , 2==右上角, 3==左上角
        float left = (1.0f - value)/2+0.01f;
        float right = 0.5f + (1.0f - value)/2-0.01f;

        m_Uvs[0] = new Vector2(left, 0.0f);
        m_Uvs[3] = new Vector2(left, 1.0f);

        m_Uvs[1] = new Vector2(right, 0.0f);
        m_Uvs[2] = new Vector2(right, 1.0f);

        m_mesh.uv = m_Uvs;
    }

    // 根据生命值状况更新生命条模型的UV位置
    public void UpdateLife(float currentlife, float maxlife)
    {
        if (m_maxLife == 0)
            return;

        m_currentLife = currentlife;
        m_maxLife = maxlife;
        this.Pad(currentlife / maxlife);

        m_transform.localScale = new Vector3(m_hscale, m_vscale, 1.0f);
    }

    // 设置生命条的位置和方向，它将始终面向摄像机
    public void SetPosition( Vector3 position, float yoffset )
    {
        Vector3 vec = position;
        vec.y += yoffset;
        m_transform.position = vec;

        // 面向Camera
        Vector3 rot = new Vector3();
        rot.y = m_cameraTransform.eulerAngles.y;
        rot.x = m_cameraTransform.eulerAngles.x;
        m_transform.eulerAngles = rot;

    }

 

}
