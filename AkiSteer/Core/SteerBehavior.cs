using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Kurisu.AkiSteer
{
[System.Serializable]
public class SteerData 
{
    [HorizontalGroup]
    public List<Transform> targets = new List<Transform>();
    [HorizontalGroup]
    public List<Collider> obstacles = new List<Collider>();
    [DisplayAsString]
    public Transform currentTarget;

    public int GetTargetsCount() => targets == null ? 0 : targets.Count;
}
public abstract class SteerBehavior : MonoBehaviour
{
    public abstract (float[] danger, float[] interest) 
        GetSteering(float[] danger, float[] interest, SteerData aiData);
}
}