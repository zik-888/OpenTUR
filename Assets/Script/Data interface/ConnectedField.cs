using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectedField : MonoBehaviour {
    
    [SerializeField] protected Text textField;

    protected string[] content = new string[10];

    public void SetTextField(string value)
    {
        for (int i = content.Length - 1; i > 0; i--)
        {
            content[i] = content[i - 1];
        }

        content[0] = value;

        textField.text = "";

        foreach (string s in content)
        {
            textField.text += s + " \r\n";
        }

        
    }
}
