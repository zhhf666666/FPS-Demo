using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Vector3 xyzCount = Vector3.zero;
    public Vector3 FloorSize = Vector3.zero;
    public List<Transform> floors = new List<Transform>();
    public List<Transform> walls = new List<Transform>();

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
    
    [ContextMenu("Reset Wall Layout (Special)")]
    private void ResetWallLayout()
    {
        walls.Clear();
        var children = this.GetComponentsInChildren<Transform>();
        for(int i=1;i<children.Length;i++)
        {
            if(children[i].gameObject.CompareTag("Wall"))
            {
                children[i].localEulerAngles = Vector3.zero;
                walls.Add(children[i]);
            }
        }
        int index = 0;
        for(int j=0;j<20;j++)
        {
            Vector3 pos1 = new Vector3(-30.255f, 0, -30+3*j);
            Vector3 pos2 = new Vector3(30.255f, 0, -30+3*j);
            walls[index].localPosition = pos1;
            walls[index+1].localPosition = pos2;
            index += 2;
        }
        for(int j=0;j<20;j++)
        {
            Vector3 pos1 = new Vector3(-30+3*j, 0, -30.255f);
            Vector3 pos2 = new Vector3(-30+3*j, 0, 30.255f);
            Vector3 angle = new Vector3(0, 90, 0);
            walls[index].Rotate(angle);
            walls[index+1].Rotate(angle);
            walls[index].localPosition = pos1;
            walls[index+1].localPosition = pos2;
            index += 2;
        }
    }
}
