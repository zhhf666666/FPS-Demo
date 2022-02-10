using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Vector3 xyzCount = Vector3.zero;
    public Vector3 FloorSize = Vector3.zero;
    public List<Transform> floors = new List<Transform>();

    [ContextMenu("Reset Floor Layout")]
    private void ResetFloorLayout()
    {
        floors.Clear();
        var children = this.GetComponentsInChildren<Transform>();
        for(int i=1;i<children.Length;i++)
        {
            if(children[i].gameObject.CompareTag("Floor"))
                floors.Add(children[i]);
        }
        int index = 0;
        for(int i=0;i<xyzCount.x;i++)
        {
            for(int j=0;j<xyzCount.z;j++)
            {
                Vector3 pos = new Vector3(i*FloorSize.x, 0, j*FloorSize.z);
                floors[index].localPosition = pos;
                index++;
            }
        }
    }
}
