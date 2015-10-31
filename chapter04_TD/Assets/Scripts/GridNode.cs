using UnityEngine;
using System.Collections;

//定义场景信息
[System.Serializable]
public class MapData
{
    public enum FieldTypeID
    {
        // 可以放置防守单位
        GuardPosition,
        //不可以放置防守单位
        CanNotStand,
    }
    //默认可以放置防守单位
    public FieldTypeID fieldtype = FieldTypeID.GuardPosition;
}

public class GridNode : MonoBehaviour 
{
    public MapData _mapData;

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(this.transform.position, "gridnode.tif");
    }

}
