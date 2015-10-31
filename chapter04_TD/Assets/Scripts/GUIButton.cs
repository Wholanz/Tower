using UnityEngine;
using System.Collections;

public class GUIButton : MonoBehaviour 
{
    // button state
    protected enum StateID
    {
        NORMAL=0,   // 正常
        FOCUS,      // Focus
        ACTIV,      // 选中
    }
    protected StateID m_state = StateID.NORMAL;

    /// <summary>
    /// texture ,0 - normal ,1 -focus ,2- activ
    /// </summary>
    public Texture[] m_ButtonSkin;

    // 按钮的ID
    public int m_ID = 0;

    // 按钮是否处于始终激活状态
    protected bool m_isOnActiv = false;

 
    // 按钮的缩放
    [HideInInspector]
    public float m_scale = 1.0f;

    Vector2 m_screenPosition;
 

    public GUITexture m_texture;

   
    void Awake()
    {
        // cache
        m_texture = this.guiTexture;

        // 获得位置
        m_screenPosition = new Vector3(m_texture.pixelInset.x, m_texture.pixelInset.y, 0);

        // set default state
        SetState(StateID.NORMAL);
    }

    /// <summary>
    /// 更新按钮状态
    /// </summary>
    /// <returns></returns>
    public int UpdateState(bool mouse, Vector3 mousepos)
    {
        int result = -1;

        if (m_texture.HitTest(mousepos))
        {
            if (mouse)
            {
                SetState(StateID.ACTIV);

                return m_ID;
            }
            else
            {
                SetState(StateID.FOCUS);
            }

        }
        else
        {
            if (m_isOnActiv)
            {
                SetState(StateID.ACTIV);
            }
            else
            {
                SetState(StateID.NORMAL);
            }
        }

        return result;
    }


    // 设置按钮状态
    protected virtual void SetState(StateID state)
    {
        if (m_state == state)
            return;

        m_state = state;

        m_texture.texture = m_ButtonSkin[(int)m_state];

        float w = m_ButtonSkin[(int)m_state].width * m_scale;
        float h = m_ButtonSkin[(int)m_state].height * m_scale;

        m_texture.pixelInset = new Rect(this.m_screenPosition.x, m_screenPosition.y, w, h);
    }

    // 设置缩放
    public virtual void SetScale(float scale)
    {
        m_scale = scale;

        float w = m_ButtonSkin[0].width * scale;
        float h = m_ButtonSkin[0].height * scale;

        m_screenPosition.x *= scale;
        m_screenPosition.y *= scale;

        m_texture.pixelInset = new Rect(m_screenPosition.x, m_screenPosition.y, w, h);

    }


    // 是否始终处于高亮激活状态
    public virtual void SetOnActiv(bool isactiv)
    {
        if (isactiv)
        {
            SetState(StateID.ACTIV);
        }
        else if (m_isOnActiv)
        {
            SetState(StateID.NORMAL);
        }

        m_isOnActiv = isactiv;
    }
 
}
