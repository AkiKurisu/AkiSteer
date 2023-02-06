using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Kurisu.AkiSteer
{
public class SeekBehaviour : SteerBehavior
{
    [SerializeField]
    private bool showGizmo = true;
    private Vector3 targetPositionCached;
    private float[] interestsTemp;
    [LabelText("环绕移动"),SerializeField]
    private bool sideShape;
    [LabelText("保持安全距离"),SerializeField]
    private bool useSafeDistance;
    [LabelText("安全距离"),SerializeField,ShowIf("useSafeDistance"),Tooltip("和目标距离小于安全距离时方向系数减小,反之恒定为1")]
    private float safeDistance=3f;
    [LabelText("优先系数"),SerializeField,Tooltip("方向系数倍率")]
    private float priority=1;
    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, SteerData aiData)
    {
       
        if (aiData.targets == null || aiData.targets.Count <= 0)
        {
            aiData.currentTarget = null;
            return (danger, interest);
        }
        else
        {
            if(aiData.currentTarget==null)
            {
                aiData.currentTarget = aiData.targets.OrderBy
                    (target => Vector3.Distance(target.position, transform.position)).FirstOrDefault();
            }
           targetPositionCached=aiData.currentTarget.position;
        }
        Vector3 directionToTarget = (targetPositionCached - transform.position);
        directionToTarget.y=0;
        SteerUtility.SideEnhanceCaculate(directionToTarget,interest,sideShape,useSafeDistance,safeDistance);
        SteerUtility.PriorityCaculate(interest,priority);
        interestsTemp = interest;
        return (danger, interest);
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showGizmo == false) return;
        if (Application.isPlaying && interestsTemp != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetPositionCached, 0.4f);
            if (interestsTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < interestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.directions[i] * interestsTemp[i]*2*priority);
                }
            }
        }
    }
    #endif
}

}