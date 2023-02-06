using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Kurisu.AkiSteer
{
/// <summary>
/// 要使用转向行为的追敌功能，你需要有一个容器脚本继承该接口，这样SteerController才可以获得目标
/// </summary>
public interface ISteerModel
{
    public Transform Target{get;}
}
public class SteerController : MonoBehaviour
{
    private ISteerModel model;
    [SerializeField,InlineProperty,HideLabel,ReadOnly]
    private SteerData Data=new SteerData();
    [SerializeField,LabelText("转向行为")]
    private List<SteerBehavior> steerBehaviors=new List<SteerBehavior>();
    [SerializeField,LabelText("障碍检测器")]
    private Detector detector;
    [SerializeField,LabelText("方向解决器")]
    private DirectionSolver solver;
    private void Awake() {
        model=GetComponent<ISteerModel>();
    }
    private void PerformDetection()
    {
        detector.Detect(Data);
    }
    private void Update()
    {
        if(model==null)return;
        //Enemy AI movement based on Target availability
        Data.targets.Clear();
        if(model.Target!=null)
        {
            Data.targets.Add(model.Target);
        }
        if (Data.GetTargetsCount() > 0)
        {
            Data.currentTarget = Data.targets[0];
        }
    }
    private void FixedUpdate() {
        PerformDetection();
    }
    public Vector3 SteerMove()       
    {
        return solver.GetDirectionToMove(steerBehaviors, Data);
    }
}
}