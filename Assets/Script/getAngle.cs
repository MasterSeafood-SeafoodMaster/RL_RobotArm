using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class getAngle : MonoBehaviour
{
    public Transform[] t = new Transform[4];
    public TextMeshProUGUI[] tmpT = new TextMeshProUGUI[4];
    private void FixedUpdate()
    {
        //loat Base = t[0].localEulerAngles.z - 180;
        //Vector3 angle = new Vector3(t[0].localEulerAngles.x, t[0].localEulerAngles.y, Base);
        //pT[0].text = ((int)Base).ToString();

        float ang = ClampSingleAngle(t[0].localEulerAngles.z);
        tmpT[0].text = ((int)ang).ToString();
        for (int i=1; i<4; i++)
        {
            ang = ClampSingleAngle(t[i].localEulerAngles.y);
            tmpT[i].text = ((int)ang).ToString();
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

        return Mathf.Clamp(angle, -90f, 90f);
    }
    Vector3 ClampVector3(Vector3 eulerRotation)
    {
        float clampedX = ClampSingleAngle(eulerRotation.x);
        float clampedY = ClampSingleAngle(eulerRotation.y);
        float clampedZ = ClampSingleAngle(eulerRotation.z);

        return new Vector3(clampedX, clampedY, clampedZ);
    }
}
