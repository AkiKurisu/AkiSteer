using UnityEngine;
using PathCreation;
using Sirenix.OdinInspector;
namespace Kurisu.AkiSteer.Extend.Path
{
    public class SteerFollowPathBehavior : SteerBehavior
    {
        [SerializeField]
        private bool showGizmo = true;
        [SerializeField]
        private PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        [SerializeField,Tooltip("该数值影响路径渐进的速度")]
        private float speed = 1;
        [SerializeField]
        private float startOffSet;
        private float distanceTravelled;
        private Vector3 targetPositionCached;
        private float[] interestsTemp;
        [LabelText("环绕移动"),SerializeField]
        private bool sideShape;
        [LabelText("保持安全距离"),SerializeField]
        private bool useSafeDistance;
        [LabelText("安全距离"),SerializeField,ShowIf("useSafeDistance"),Tooltip("和目标距离小于安全距离时方向系数减小,反之恒定为1")]
        private float safeDistance=3f;
        private void Awake() {
            distanceTravelled=startOffSet;
        }
        Vector3 GetPosition()
        {
            distanceTravelled += speed * Time.deltaTime;
            Vector3 pos=pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            return pos;
        }
        public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, SteerData aiData)
        {
            targetPositionCached=GetPosition();
            var pathPos=targetPositionCached-transform.position;
            SteerUtility.SideEnhanceCaculate(pathPos,interest,sideShape,useSafeDistance,safeDistance);
            interestsTemp = interest;
            return (danger, interest);
        }
    
        private void OnDrawGizmos()
        {
            if (showGizmo == false)return;
            if (Application.isPlaying && interestsTemp != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(targetPositionCached, 0.4f);
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
    }
}
