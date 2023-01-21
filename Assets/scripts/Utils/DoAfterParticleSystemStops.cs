using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class DoAfterParticleSystemStops : UdonSharpBehaviour
{

    private bool initiated = false;
    private ParticleSystem ps;
    private Command cmd;

    public DoAfterParticleSystemStops Exec(ParticleSystem ps, Command cmd)
    {
        this.ps = ps;
        this.cmd = cmd;
        this.initiated = true;
        return this;
    }

    void Update()
    {
        if (this.initiated)
        {
            if (!this.ps.isPlaying)
            {
                this.initiated = false;
                this.cmd.Exec();
            }
        }
    }

}