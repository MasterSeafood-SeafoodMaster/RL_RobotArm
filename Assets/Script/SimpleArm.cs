using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;
using Random = UnityEngine.Random;

public class SimpleArm : Agent
{
    public Rigidbody[] rNode = new Rigidbody[4];
    public Transform[] nodes = new Transform[4];
    public Transform top;
    public Transform Target;
    public Transform floor;
    public float Speed = 5f;
    public int maxstep = 5000;

    //TODO
    //1. Rotation range (-90, 90)
    //2. Top < 0 = Lose
    //3. show current angle


    public override void OnEpisodeBegin()
    {
        Target.localPosition = GenerateRandomVector();
        maxstep = 5000;
        
    }
    public override void CollectObservations(VectorSensor sensor) //value[3*2+3] = 9
    {
        sensor.AddObservation(ClampSingleAngle(nodes[0].localEulerAngles.z) / 90);
        sensor.AddObservation( ClampSingleAngle(nodes[1].localEulerAngles.x) / 90 );
        sensor.AddObservation( ClampSingleAngle(nodes[2].localEulerAngles.x) / 90 );
        sensor.AddObservation( ClampSingleAngle(nodes[3].localEulerAngles.x) / 90 );
        sensor.AddObservation( Target.localPosition/7 );
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3[] controlSignals = new Vector3[4];
        controlSignals[0] = new Vector3(0, 0, actions.ContinuousActions[0]);
        controlSignals[1] = new Vector3(0, actions.ContinuousActions[1], 0);
        controlSignals[2] = new Vector3(0, actions.ContinuousActions[2], 0);
        controlSignals[3] = new Vector3(0, actions.ContinuousActions[3], 0);


        rNode[0].angularVelocity = Vector3.zero;
        Vector3 newangle = nodes[0].localEulerAngles + (controlSignals[0] * Speed);
        newangle = new Vector3(newangle.x, newangle.y, ClampSingleAngle(newangle.z));
        nodes[0].localEulerAngles = newangle;
        for (int i=1; i<4; i++)
        {
            rNode[i].angularVelocity = Vector3.zero;
            newangle= nodes[i].localEulerAngles + (controlSignals[i] * Speed);
            newangle = new Vector3(newangle.x, ClampSingleAngle(newangle.y), newangle.z);
            nodes[i].localEulerAngles = newangle;
        }

        float distance = Vector3.Distance(top.position, Target.position);
        Debug.Log(distance);
        if (distance < 0.5f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        else if ((maxstep < 0) | (top.position.y < floor.position.y))
        {
            //SetReward(-(distance/8));
            for (int i = 0; i < 4; ++i)
            {
                rNode[i].velocity = Vector3.zero;
                rNode[i].angularVelocity = Vector3.zero;
                nodes[i].localEulerAngles = Vector3.zero;
            }
            EndEpisode();
        }
        maxstep -= 1;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[2] = Input.GetAxis("Horizontal");
        continuousActionsOut[3] = Input.GetAxis("Vertical");
        continuousActionsOut[0] = Input.GetAxis("Mouse X");
        continuousActionsOut[1] = Input.GetAxis("Mouse Y");

        //Debug.Log(continuousActionsOut);
        
    }
    Vector3 GenerateRandomVector()
    {
        float minDistance = 4f;
        float maxDistance = 10f;
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomDistance = Random.Range(minDistance, maxDistance);

        float x = randomDistance * Mathf.Cos(randomAngle);
        float z = randomDistance * Mathf.Sin(randomAngle);

        float y = Random.Range(0f, maxDistance - minDistance) + minDistance;

        return new Vector3(x, y, z);
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
}
