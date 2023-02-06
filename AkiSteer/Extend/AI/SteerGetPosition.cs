using UnityEngine;
using Kurisu.AkiBT;
namespace Kurisu.AkiSteer.Extend.AI
{
    [AkiInfo("Action:转向行为获取移动位置")]
    [AkiLabel("Steer:GetPosition")]
    [AkiGroup("Steer")]
    public class SteerGetPosition : Action
    {
        [SerializeField]
        private SteerController steerController;
        [SerializeField]
        private SharedVector3 storeResult;
        public override void Awake()
        {
            InitVariable(storeResult);
            if(steerController==null)steerController=gameObject.GetComponent<SteerController>();
        }
        protected override Status OnUpdate()
        {
            storeResult.Value=steerController.SteerMove()+gameObject.transform.position;
            return Status.Success;
        }
    }
}
