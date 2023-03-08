using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class SmokeAnimFinishCommand : Command
{

    private ShellBehaviour shellBehaviour;
    private Command cmd;
    public SmokeAnimFinishCommand Init(
        ShellBehaviour shellBehaviour, 
        Command cmd = null
    )
    {
        this.shellBehaviour = shellBehaviour;
        this.cmd = cmd;
        return this;
    }

    public override void Exec()
    {
        this.shellBehaviour.ReturnShell();
    }

}
