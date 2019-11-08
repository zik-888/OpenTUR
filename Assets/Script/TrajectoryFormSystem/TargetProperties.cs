using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetProperties : MonoBehaviour {

    [SerializeField] protected InputField inputFieldA, inputFieldB, inputFieldX, inputFieldY, inputFieldZ;
    [SerializeField] protected Slider slider1, slider2, slider3, slider4, slider5, slider6;
    [SerializeField] protected InputField speedInputField;
    public int speed = 100;

    public void SetSpeed()
    {
        speed = int.Parse(speedInputField.text);
    }

    public Vector5 GetInputField()
    {
        Vector3 vector3 = new Vector3(float.Parse(inputFieldX.text), float.Parse(inputFieldY.text), float.Parse(inputFieldZ.text));
        Vector2 vector2 = new Vector2(float.Parse(inputFieldA.text), float.Parse(inputFieldB.text));

        return new Vector5(vector3, vector2, slider6.value);
    }

    public void SetInputField(Vector5 vector)
    {
        inputFieldX.text = vector.X.ToString();
        inputFieldY.text = vector.Y.ToString();
        inputFieldZ.text = vector.Z.ToString();
        inputFieldA.text = vector.A.ToString();
        inputFieldB.text = vector.B.ToString();
    }

    public Joint6 GetSlider()
    {
        return new Joint6(slider1.value, slider2.value, slider3.value, slider4.value, slider5.value, slider6.value);
    }

    public void SetSlider(Joint6 joint)
    {
        slider1.value = joint.J1;
        slider2.value = joint.J2;
        slider3.value = joint.J3;
        slider4.value = joint.J4;
        slider5.value = joint.J5;
        slider6.value = joint.J6;
    }

    public void SetProp3Slider(float minLine, float maxLine)
    {
        slider3.minValue = minLine;
        slider3.maxValue = maxLine;
    }

}
