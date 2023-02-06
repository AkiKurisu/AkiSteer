using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Kurisu.AkiSteer
{
public class Detector : MonoBehaviour
{
    [SerializeField,LabelText("检测范围")]
    private float detectionRadius = 2;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private bool showGizmos = true;
    Collider[] colliders=new Collider[30];
    [LabelText("忽略碰撞体"),SerializeField]
    private List<Collider> ignoreColliders=new List<Collider>();
    private int amount;
    internal void Detect(SteerData data)
    {
        amount = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius,colliders, layerMask);
        data.obstacles.Clear();
        for(int i=0;i<amount;i++)
        {
            if(!ignoreColliders.Contains(colliders[i])&&colliders[i].transform!=data.currentTarget)
                data.obstacles.Add(colliders[i]);
        }
        
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showGizmos == false)
            return;
        if (Application.isPlaying && colliders != null)
        {
            Gizmos.color = Color.red;
            for(int i=0;i<amount;i++)
            {
                Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
            }
        }
    }
    #endif
}
}