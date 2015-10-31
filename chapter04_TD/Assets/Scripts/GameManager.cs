using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public bool m_debug = false;

    public ArrayList m_PathNodes;

    // 敌人列表
    public ArrayList m_EnemyList=new ArrayList();

    // 波数
    [HideInInspector]
    public int m_wave = 0;

    // 生命
    public int m_life = 10;

    // 点数
    public int m_point = 10;

    // 文字
    GUIText m_txt_wave;
    GUIText m_txt_life;
    GUIText m_txt_point;

    // 按钮
    GUIButton m_button;

    // 当前选中的按钮ID
    int m_ID;

    // 防守单位prefab
    public Transform m_guardPrefab;

    // 地面的碰撞层
    public LayerMask m_groundlayer;

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {

        // 获取文字
        m_txt_wave = this.transform.FindChild("txt_wave").GetComponent<GUIText>();
        m_txt_life = this.transform.FindChild("txt_life").GetComponent<GUIText>();
        m_txt_point = this.transform.FindChild("txt_point").GetComponent<GUIText>();

        // 初始化文字
        m_txt_wave.text = "<color=red>wave</color> " + m_wave;
        m_txt_life.text = "<color=red>life</color> " + m_life;
        m_txt_point.text = "<color=red>point</color> " + m_point;

        BuildPath();

        m_button = this.transform.FindChild("button_0").GetComponent<GUIButton>();
	
	}
	
	// Update is called once per frame
	void Update () {

        //如果生命为0
        if (m_life <= 0)
            return;
	
        // 按下鼠标操作
        bool press=Input.GetMouseButton(0);

        // 松开鼠标操作
        bool mouseup = Input.GetMouseButtonUp(0);

        // 获得鼠标位置
        Vector3 mousepos = Input.mousePosition;

        // 获得鼠标移动距离
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // 如果当前按钮ID大于0，并且处于松开鼠标操作
        if (m_ID > 0 && mouseup )
        {
            //如果小于5个点数，返回，什么也不做
            if (m_point < 5)
            {
                m_ID = 0;
                m_button.SetOnActiv(false);
                return;
            }

            //创建一条从摄像机射出的射线
            Ray ray = Camera.main.ScreenPointToRay(mousepos);

            //计算射线与地面的碰撞
            RaycastHit hit;
            if ( Physics.Raycast(ray, out hit, 100, m_groundlayer) )
            {
                //获得碰撞点的位置
                int ix = (int)hit.point.x;
                int iz = (int)hit.point.z;

                if (ix >= GridMap.Instance.MapSizeX || iz >= GridMap.Instance.MapSizeZ || ix<0 || iz<0 )
                    return;

                // 如果当前单元格可以摆放防守单位
                if (GridMap.Instance.m_map[ix, iz].fieldtype == MapData.FieldTypeID.GuardPosition)
                {
                    Vector3 pos = new Vector3((int)hit.point.x + 0.5f, 0, (int)hit.point.z + 0.5f);

                    // 创建防守单位
                    Instantiate(m_guardPrefab, pos, Quaternion.identity);
                    m_ID = 0;

                    // 按钮重新恢复到正常状态
                    m_button.SetOnActiv(false);

                    // 减少5个点数
                    SetPoint(-5);
                }
            }
        }

        // 获得按钮的ID
        int id = m_button.UpdateState(mouseup, Input.mousePosition);
        if (id > 0)
        {
            m_ID = id;
            m_button.SetOnActiv(true);
            return;
        }

        // 移动摄像机
        GameCamera.Inst.Control(press, mx, my);
	}

    // 更新波数
    public void SetWave(int wave)
    {
        m_wave= wave;

        m_txt_wave.text = "<color=red>wave</color> " + m_wave;

    }

    // 更新生命
    public void SetDamage(int damage)
    {
        m_life -= damage;

        m_txt_life.text = "<color=red>life</color> " + m_life;

    }

    // 更新点数
    public void SetPoint(int point)
    {
        m_point += point;

        m_txt_point.text = "<color=red>point</color> " + m_point;

    }

    [ContextMenu("BuildPath")]
    void BuildPath()
    {
        m_PathNodes = new ArrayList();

        GameObject[] objs = GameObject.FindGameObjectsWithTag("pathnode");

        for (int i = 0; i < objs.Length; i++)
        {
            PathNode node = objs[i].GetComponent<PathNode>();

            m_PathNodes.Add(node);
        }
    }

    void OnGUI()
    {
        if (m_life <= 0 || ( m_wave == 10 && m_EnemyList.Count==0))
        {
            if (GUI.Button(new Rect(Screen.width * 0.5f - 100, Screen.height * 0.5f - 25, 200, 50), "REPLAY"))
                Application.LoadLevel(Application.loadedLevelName);
        }
    }

    /// <summary>
    /// Debug
    /// </summary>
    void OnDrawGizmos()
    {
        if (!m_debug || m_PathNodes==null )
            return;

        Gizmos.color = Color.blue;

        foreach (PathNode node in m_PathNodes)
        {
            if (node.m_next != null)
            {
                Gizmos.DrawLine(node.transform.position, node.m_next.transform.position);
            }
        }
    }
}
