using UnityEngine;
using UnityEditor;
using System.Collections;

public class PathTool : ScriptableObject 
{
    static PathNode m_parent=null;

    [MenuItem("PathTool/Set Parent %q")]
    static void SetParent()
    {
      
        if (!Selection.activeGameObject || Selection.GetTransforms(SelectionMode.Unfiltered).Length>1)
            return;
       
        if (Selection.activeGameObject.tag.CompareTo("pathnode") == 0)
        {
            m_parent = Selection.activeGameObject.GetComponent<PathNode>();

            Debug.Log( "Set "+m_parent.name+" as parent.");
        }

    }

    [MenuItem("PathTool/Set Next %w")]
    static void SetNextChild()
    {
        if (!Selection.activeGameObject || m_parent==null || Selection.GetTransforms(SelectionMode.Unfiltered).Length>1)
            return;

        if (Selection.activeGameObject.tag.CompareTo("pathnode") == 0)
        {
            m_parent.SetNext( Selection.activeGameObject.GetComponent<PathNode>() );
            m_parent = null;

            Debug.Log("Set " + Selection.activeGameObject.name + " as child.");
        }
      
    }


}
       
