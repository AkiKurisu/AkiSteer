using UnityEngine;
namespace Kurisu.AkiSteer
{
    public class SteerUtility
{
    /// <summary>
    /// 边缘系数强化计算
    /// </summary>
    /// <param name="directionToTarget">目标相对于自身方向</param>
    /// <param name="interest">当前兴趣系数</param>
    /// <param name="sideShape">开启边缘强化</param>
    /// <param name="useSafeDistance">使用安全距离</param>
    /// <param name="safeDistance">安全距离</param>
    public static void SideEnhanceCaculate(Vector3 directionToTarget,float[] interest,bool sideShape,bool useSafeDistance,float safeDistance)
    {
        float distance=directionToTarget.sqrMagnitude;
        Vector3 normalizedDirection=directionToTarget.normalized;
        bool needLoss=false;
        float distanceMulti=0;
        if(useSafeDistance&&distance<safeDistance*safeDistance)
        {
            //距离衰减偏差,越近偏差越大最大为1，超过安全距离为0
            //当前距离为1.5,安全距离为3则衰减偏差为2*(1-1.5/3)=1
            //保证在安全距离一半时受到最大阻碍
            distanceMulti=Mathf.Clamp01(2*(1-distance/(safeDistance*safeDistance)));
            needLoss=true;
        }
        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector3.Dot(normalizedDirection, Directions.directions[i]);
            if(needLoss)//需要衰减
            {
                result-= (result<0.65?1:distanceMulti);//衰减仅作用部分方向
            }
            if(sideShape)
            {
                //法向量计算
                float dot=Vector3.Dot(normalizedDirection, Directions.normals[i]);
                //向一侧偏离
                float sideWeight=1-Mathf.Abs(dot-0.65f);
                result*=sideWeight;  
            }
            if(result>interest[i])interest[i] = result;     
        }
    }
    /// <summary>
    /// 优先级计算
    /// </summary>
    /// <param name="interest"></param>
    /// <param name="priority"></param>
    public static void PriorityCaculate(float[] interest,float priority)
    {
        for(int i=0;i<interest.Length;i++)interest[i]*=priority;
    }
}
}
