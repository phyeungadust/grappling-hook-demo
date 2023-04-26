using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class DoAfterAllParticleSystemsStop : UdonSharpBehaviour
{

    private Command cmd;
    private ParticleSystem[] psArr;
    private bool initiated;

    public DoAfterAllParticleSystemsStop Exec(
        Command cmd,
        params ParticleSystem[] psArr
    )
    {
        this.cmd = cmd;
        this.psArr = psArr;
        this.initiated = true;
        return this;
    }

    void Update()
    {
        if (this.initiated)
        {
            foreach (ParticleSystem ps in this.psArr)
            {
                if (ps.isPlaying)
                {
                    return;
                }
            }
            // all particle systems stopped
            // call command
            this.initiated = false;
            this.cmd.Exec();
        }
    }

}