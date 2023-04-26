using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class WaterSprayParticleProperties : UdonSharpBehaviour
{

    // public int OwnerID;
    public float ParticleTravelSpeed;
    public float ParticleLifeTime;
    public float SpraySpread;
    // public float ParticleScaleMin;
    // public float ParticleScaleMax;

    public Gradient ColorGradientOverLifetime;

}