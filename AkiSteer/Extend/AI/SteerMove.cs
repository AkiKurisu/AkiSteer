using UnityEngine;
using Kurisu.AkiBT;
namespace Kurisu.AkiSteer.Extend.AI
{
    [AkiInfo("Action:转向行为移动")]
    [AkiLabel("Steer:Move")]
    [AkiGroup("Steer")]
    public class SteerMove : Action
    {
        [SerializeField]
        private SteerController steerController;
        [SerializeField]
        private float speed=2;
        public override void Awake()
        {
            if(steerController==null)steerController=gameObject.GetComponent<SteerController>();
        }
        protected override Status OnUpdate()
        {
            Vector3 forward=steerController.SteerMove();
            Vector3 newPos=forward+gameObject.transform.position;
            gameObject.transform.position=Vector3.Lerp(gameObject.transform.position,newPos,speed*Time.deltaTime);
            gameObject.transform.forward=Vector3.Lerp(gameObject.transform.forward,forward.normalized,Time.deltaTime*speed);
            return Status.Success;
        }
    }
}
