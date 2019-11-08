using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GUIManager : MonoBehaviour {

    [SerializeField] protected ScenceControl scenceControl;
    [SerializeField] public PrefabCollection prefabCollection;
    

    protected List<Trajectory> trajectoryList = new List<Trajectory>();
    protected Vector2Int idItem;

    void Awake()
    {
        Messenger<Vector2Int>.AddListener(GameEvent.ID_ITEM, SetID);
    }

    public void AddItem(int typeItem)
    {
        switch ((TypeItem)typeItem)
        {
            case TypeItem.Line:
                trajectoryList.Add(gameObject.AddComponent<Line>());
                trajectoryList[trajectoryList.Count - 1].InitItem(trajectoryList.Count - 1, prefabCollection);
                SetID(new Vector2Int(trajectoryList.Count - 1, -1));
                break;

            case TypeItem.Spline:
                trajectoryList.Add(gameObject.AddComponent<Spline>());
                trajectoryList[trajectoryList.Count - 1].InitItem(trajectoryList.Count - 1, prefabCollection);
                SetID(new Vector2Int(trajectoryList.Count - 1, -1));
                break;

            case TypeItem.Circle:
                trajectoryList.Add(gameObject.AddComponent<Circle>());
                trajectoryList[trajectoryList.Count - 1].InitItem(trajectoryList.Count - 1, prefabCollection);
                SetID(new Vector2Int(trajectoryList.Count - 1, -1));
                break;
        }
    }

    public void RemoveItem()
    {
        if(idItem.y == -1) 
        {
            trajectoryList[idItem.x].DestrItem();
            trajectoryList.RemoveAt(idItem.x);
            SetID(new Vector2Int(trajectoryList.Count - 1, -1));

            for(int i = idItem.x; trajectoryList.Count > i; i++)
            {
                trajectoryList[i].Num = i;
            }
            
        }
        else
        {
            trajectoryList[idItem.x].RemovePoint(idItem.y);
            SetID(new Vector2Int(idItem.x, idItem.y - 1));
        }
        
    }
    
    public void SetID(Vector2Int value)
    {

        idItem = value;

        if (idItem.y != -1)
        {
            trajectoryList[idItem.x].idPoint = idItem.y;
            scenceControl.SetOptionPoint(trajectoryList[idItem.x].GetOptionPoint());
        }
    }

    public void WORKProgram()
    {
        List<Vector5> vectorList = new List<Vector5>();

        for (int i = 0; trajectoryList.Count > i; i++)
        {
            vectorList.AddRange(trajectoryList[i].GetApproximatedArray());

            //if(i != trajectoryList.Count - 1)
            //{
            //    vectorList.AddRange(trajectoryList[i + 1].GetApproximatedArray());
            //}
        }

        //SetID(new Vector2Int(trajectoryList.Count - 1, trajectoryList[trajectoryList.Count - 1].pointList.Count - 1));

        scenceControl.ProgramVectors = vectorList.ToArray();
        scenceControl.workProgram = true;
        //SetID(new Vector2Int(trajectoryList.Count - 1, trajectoryList[trajectoryList.Count - 1].pointList.Count - 1));
        //trajectoryList[trajectoryList.Count - 1].idPoint = trajectoryList[trajectoryList.Count - 1].pointList.Count - 1;
        //scenceControl.SetOptionPoint(trajectoryList[trajectoryList.Count - 1].GetOptionPoint());
    }

    public void UpdateOptionPoint()
    {
        try
        {
            trajectoryList[idItem.x].SetOptionPoint(scenceControl.targetProperties.GetInputField());
        }
        catch
        {

        }
        
    }

}
