using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenceEvent : MonoBehaviour {
    
    [SerializeField] protected GameObject pointer;
    [SerializeField] protected GameObject prop;
    [SerializeField] protected Button menuButton;
    [SerializeField] protected RectTransform menuCanvas;
    [SerializeField] protected RectTransform dynamicCanvas;
    [SerializeField] protected Image contentObject;


    public void ClosePointerField()
    {
        pointer.SetActive(!pointer.activeSelf);
    }

    public void ClosePropField()
    {
        prop.SetActive(!prop.activeSelf);
    }

    public void MenuButton()
    {
        menuCanvas.gameObject.SetActive(!menuCanvas.gameObject.activeSelf);
    }

    public void DynamicButton()
    {
        dynamicCanvas.gameObject.SetActive(!dynamicCanvas.gameObject.activeSelf);
    }

    public void CloseContentObject()
    {
        contentObject.gameObject.SetActive(!contentObject.gameObject.activeSelf);
            
    }
    
}
