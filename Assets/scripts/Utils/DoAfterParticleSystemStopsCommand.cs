using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class DoAfterParticleSystemStopsCommand : Command
{

    private DoAfterParticleSystemStops doAfterPSStops;
    private ParticleSystem ps;
    private Command cmd;

    public DoAfterParticleSystemStopsCommand Init(
        DoAfterParticleSystemStops doAfterPSStops,
        ParticleSystem ps,
        Command cmd
    )
    {
        this.doAfterPSStops = doAfterPSStops;
        this.ps = ps;
        this.cmd = cmd;
        return this;
    }

    public override void Exec()
    {
        this.doAfterPSStops.Exec(this.ps, this.cmd);
    }

}