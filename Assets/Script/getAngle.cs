using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class getAngle : MonoBehaviour
{
    public Transform[] t = new Transform[4];
    public TextMeshProUGUI[] tmpT = new TextMeshProUGUI[4];
    void Update()
    {
        Vector3 angle = new Vector3(t[0].localEulerAngles.x, t[0].localEulerAngles.y, t[0].localEulerAngles.z);
        tmpT[0].text = (ClampSingleAngle(angle.z)).ToString();
        for (int i=1; i<4; i++)
        {
            tmpT[i].text = ClampSingleAngle(t[i].localEulerAngles.y).ToString();
            //tmpT[i].text = ClampVector3(t[i].localEulerAngles).ToString();
        }
    }
    float ClampSingleAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }
        else if (angle < -180f)
        {
            angle += 360f;
        }

        return (int)Mathf.Clamp(angle, -90f, 90f);
    }

}
