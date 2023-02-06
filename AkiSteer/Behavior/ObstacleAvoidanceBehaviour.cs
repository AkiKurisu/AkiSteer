using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Kurisu.AkiSteer
{
public class ObstacleAvoidanceBehaviour : SteerBehavior
{
    [SerializeField]
    private float radius = 2f;
    [SerializeField]
    private float agentColliderSize = 0.6f;

    [SerializeField]
    private bool showGizmo = true;

    //gizmo parameters
    [SerializeField,ReadOnly,InlineProperty]
    float[] dangersResultTemp = null;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, SteerData aiData)
    {
        if(aiData.obstacles.Count!=0)
        {foreach (Collider obstacleCollider in aiData.obstacles)
        {
            Vector3 directionToObstacle
                = obstacleCollider.ClosestPoint(transform.position) - transform.position;
            directionToObstacle.y=0;
            float distanceToObstacle = directionToObstacle.magnitude;

            //calculate weight based on the distance Enemy<--->Obstacle
            float weight
                = distanceToObstacle <= agentColliderSize
                ? 1
                : (radius - distanceToObstacle) / radius;

            Vector3 directionToObstacleNormalized = directionToObstacle.normalized;

            //Add obstacle parameters to the danger array
            for (int i = 0; i < Directions.directions.Count; i++)
            {
                float result = Vector3.Dot(directionToObstacleNormalized, Directions.directions[i]);

                float valueToPutIn = result * weight;

                //override value only if it is higher than the current one stored in the danger array
                if (valueToPutIn > danger[i])
                {
                    danger[i] = valueToPutIn;
                }
            }
        }}
        dangersResultTemp = danger;
        return (danger, interest);
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showGizmo == false)
            return;

        if (Application.isPlaying && dangersResultTemp != null)
        {
            if (dangersResultTemp != null)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < dangersResultTemp.Length; i++)
                {
                    Gizmos.DrawRay(
                        transform.position,
                        Directions.directions[i] * dangersResultTemp[i]*2
                        );
                }
            }
        }
    }
    #endif
}

public static class Directions
{
    public static List<Vector3> directions = new List<Vector3>{
            new Vector3(0,0,1).normalized,
            new Vector3(1,0,1).normalized,
            new Vector3(1,0,0).normalized,
            new Vector3(1,0,-1).normalized,
            new Vector3(0,0,-1).normalized,
            new Vector3(-1,0,-1).normalized,
            new Vector3(-1,0,0).normalized,
            new Vector3(-1,0,1).normalized,
            new Vector3(0,1,0).normalized,//向上矢量
            new Vector3(0,-1,0).normalized//向下矢量
        };
    public static List<Vector3> normals = new List<Vector3>{
            new Vector3(-1,0,0).normalized,
            new Vector3(-1,0,1).normalized,
            new Vector3(0,0,1).normalized,
            new Vector3(1,0,1).normalized,
            new Vector3(1,0,0).normalized,
            new Vector3(1,0,-1).normalized,
            new Vector3(0,0,-1).normalized,
            new Vector3(-1,0,-1).normalized,
            new Vector3(1,0,0).normalized,
            new Vector3(-1,0,0).normalized
        };
}

}
