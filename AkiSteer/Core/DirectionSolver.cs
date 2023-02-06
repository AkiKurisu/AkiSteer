using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Kurisu.AkiSteer
{
public class DirectionSolver : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos = true;

    //gozmo parameters
    float[] interestGizmo = new float[10];
    Vector3 resultDirection = Vector3.zero;
    private float rayLength = 2;
    [ShowInInspector,ReadOnly,HorizontalGroup]
    float[] danger = new float[10];
    [ShowInInspector,ReadOnly,HorizontalGroup]
    float[] interest = new float[10];

    internal Vector3 GetDirectionToMove(IEnumerable<SteerBehavior> behaviours, SteerData aiData)
    {
        for(int i=0;i<Directions.directions.Count;i++)
        {
            danger[i]=0;
            interest[i]=0;
        }
        //Loop through each behaviour
        foreach (SteerBehavior behaviour in behaviours)
        {
            (danger, interest) = behaviour.GetSteering(danger, interest, aiData);
        }

        //subtract danger values from interest array
        for (int i = 0; i < interest.Length; i++)
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        interestGizmo = interest;

        //get the average direction
        Vector3 outputDirection = Vector3.zero;
        for (int i = 0; i < Directions.directions.Count; i++)
        {
            outputDirection += Directions.directions[i] * interest[i];
        }

        outputDirection.Normalize();

        resultDirection = outputDirection;

        //return the selected movement direction
        return resultDirection;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying && showGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, resultDirection * rayLength);
        }
    }
    #endif
}
}