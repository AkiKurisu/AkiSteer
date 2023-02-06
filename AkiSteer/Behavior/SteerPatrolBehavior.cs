using UnityEngine;
using Sirenix.OdinInspector;
namespace Kurisu.AkiSteer
{
public class SteerPatrolBehavior : SteerBehavior
{
    [SerializeField]
    private bool showGizmo=true;
    public bool isUse=false;
    [SerializeField]
    private float patrolRadius;
    [SerializeField]
    private Vector3 patrolCenter;
    [SerializeField]
    private bool useStartPointAsCenter;
    private float[] interestsTemp;
    [LabelText("柏林噪声"),SerializeField,ReadOnly]
    float perrin;
    [LabelText("距离系数"),SerializeField]
    private float distanceMulti;
    [SerializeField,LabelText("更新时延")]
    private float updateDelay=0.5f;
    private float timer;
    Vector3 newDirection;
    Vector3 fixDirection;
    private void Start() {
        if(useStartPointAsCenter)
            patrolCenter=transform.position;
    }
     public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, SteerData aiData)
    {
        if(!isUse)
        return (danger,interest);
        else
        {
            for (int i = 0; i < interest.Length; i++)
            {
                float result = Vector3.Dot(fixDirection.normalized, Directions.directions[i]);
                if(result>interest[i])
                    interest[i] = result;
            }
            interestsTemp=interest;
            return (danger, interest);
        }
    }
    private void Update() {
        if(!isUse)return;
        timer+=Time.deltaTime;
        if(timer>updateDelay)
        {
            timer=0;
            perrin=Mathf.PerlinNoise(transform.position.x,transform.position.z);
            newDirection=transform.forward.normalized+transform.right.normalized*(perrin-0.5f);
            newDirection.y=0;
            distanceMulti=Mathf.Clamp((transform.position-patrolCenter).magnitude/patrolRadius,0,3);
            fixDirection=(newDirection/(Mathf.Max(distanceMulti,1))+(distanceMulti>0.5f?distanceMulti:0)*((patrolCenter-transform.position).normalized));
        }
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        if (showGizmo == false)
            return;
        if (Application.isPlaying && interestsTemp != null&&isUse)
        {
            Gizmos.DrawWireSphere(patrolCenter,patrolRadius);
            if (interestsTemp != null)
            {
                Gizmos.color = Color.blue;
                for (int i = 0; i < interestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.directions[i] * interestsTemp[i]*2);
                }
                
            }
        }
    }
    #endif
}
}