using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrItem : MonoBehaviour {

    public int num;

    public void SendAddPoint()
    {
        Messenger<int>.Broadcast(GameEvent.ADD_POINT_IN_TRAJECTORY, num);
    }
}
