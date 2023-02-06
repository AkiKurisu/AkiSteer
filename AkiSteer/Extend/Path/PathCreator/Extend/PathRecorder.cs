using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace PathCreation.Extend
{
    [TypeInfoBox("路径记录器")]
    public class PathRecorder : MonoBehaviour
    {
        public enum RecordState
        {
            未记录,记录中
        }
        [SerializeField,ReadOnly]
        private RecordState state;
        public RecordState CurrentState=>state;
        [SerializeField,LabelText("记录间隔")]
        private float recordTime=1;
        [SerializeField,LabelText("记录路径")]
        private List<Vector3> pathPoints=new List<Vector3>();
        [SerializeField,LabelText("记录上限"),Tooltip("记录数达到上限后移除首部")]
        private float maxRecordLength=50;
        [SerializeField,LabelText("记录下限"),Tooltip("记录数达到下限后开始输出")]
        private float startRecordLength=10;
        [SerializeField,LabelText("输出路径")]
        private PathCreator pathCreator;
        [SerializeField,LabelText("开始时记录")]
        private bool recordWhenStart;
        [SerializeField]
        private Vector3 offSet=Vector3.up;
        private void Start() {
            if(recordWhenStart)StartRecord();
        }
        /// <summary>
        /// 进入自动记录模式
        /// </summary>
        [Button("记录路径",ButtonSizes.Medium),DisableIf("state",RecordState.记录中),DisableInEditorMode,ButtonGroup,GUIColor(0f,1f,0)]
        public void StartRecord()
        {
            pathCreator.bezierPath.IsClosed=false;
            state=RecordState.记录中;
        }
        /// <summary>
        /// 手动记录,记录中时无效
        /// </summary>
        public void ManulRecord(Vector3 vector3)
        {
            if(state==RecordState.记录中)return;
            pathCreator.bezierPath.IsClosed=false;
            AddPoint(vector3);
            pathCreator.bezierPath.IsClosed=true;
        }
        /// <summary>
        /// 结束自动记录
        /// </summary>
        [Button("停止记录",ButtonSizes.Medium),DisableIf("state",RecordState.未记录),DisableInEditorMode,ButtonGroup,GUIColor(1,0f,0)]
        public void EndRecord()
        {
            pathCreator.bezierPath.IsClosed=true;
            state=RecordState.未记录;
        }
        [Button("生成路径",ButtonSizes.Medium),DisableInEditorMode,GUIColor(0.4f,0.8f,1)]
        public void CreatePath()
        {
            pathCreator.bezierPath = new BezierPath (pathPoints, false, PathSpace.xyz);
        }
        private float timer;
        private void FixedUpdate() {
            if(state!=RecordState.记录中)return;
            timer+=Time.fixedDeltaTime;
            if(timer>recordTime)
            {
                timer=0;
                AddPoint(transform.position+offSet);
            }
        }
        /// <summary>
        /// 向路径生成器添加点
        /// </summary>
        /// <param name="vector3"></param>
        private void AddPoint(Vector3 vector3)
        {
            pathPoints.Add(vector3);
            int Count=pathPoints.Count;
            if(Count>maxRecordLength){
                //去除队列首部
                pathPoints.RemoveAt(0);
                Count--;
            }
            //到达下限生成路径
            if(Count==startRecordLength)CreatePath();
            if(Count>startRecordLength)pathCreator.bezierPath.AddSegmentToEnd(pathPoints[Count-1]);
        }
    }
}
