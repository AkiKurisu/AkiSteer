using UnityEngine;
using Sirenix.OdinInspector;
namespace PathCreation.Extend
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    [TypeInfoBox("路径跟随器")]
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        [ShowInInspector,ReadOnly]
        float distanceTravelled;
        [SerializeField]
        private float offset;
        [SerializeField]
        private bool lerpFollow;
        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
            distanceTravelled=offset;
        }

        void Update()
        {
            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                Vector3 pos=pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                Quaternion rot=pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                if(lerpFollow)
                {
                    transform.position = Vector3.Lerp(transform.position,pos,Time.deltaTime*speed);
                    transform.rotation = Quaternion.Lerp(transform.rotation,rot,Time.deltaTime*speed);
                    return;
                }
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}