using UnityEngine;
using System.Collections;

//���峡����Ϣ
[System.Serializable]
public class MapData
{
    public enum FieldTypeID
    {
        // ���Է��÷��ص�λ
        GuardPosition,
        //�����Է��÷��ص�λ
        CanNotStand,
    }
    //Ĭ�Ͽ��Է��÷��ص�λ
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
