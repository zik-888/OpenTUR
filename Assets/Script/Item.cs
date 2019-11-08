using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public enum TypeItem { Point, Line, Spline, Circle };

public class Item : MonoBehaviour {
    
    public Vector2Int gNum;
    
    public void SelectItem()
    {
        Messenger<Vector2Int>.Broadcast(GameEvent.ID_ITEM, gNum);
    }
}
