using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class ShellBehaviour : UdonSharpBehaviour
{

    [SerializeField]
    public VRCObjectPool ShellPool;
    [SerializeField]
    private Tether.TetherController leftController;
    [SerializeField]
    private Tether.TetherController rightController;
    [SerializeField]
    private ShellProperties shellProperties;
    [SerializeField]
    private ShellShootProperties shellShootProperties;
    [SerializeField]
    private ParticleSystem smoke;

    private bool initialized = false;
    private int shooterID = -1;
    private int victimID = -1;
    private VRCPlayerApi localPlayer;

    private DoAfterSmokeAnimFinish doAfterSmokeAnimFinish;
    private ReturnShellCommand returnShellCommand;

    [UdonSynced, FieldChangeCallback(nameof(Init))]
    private string init = "";
    public string Init
    {
        get => this.init;
        set
        {

            this.init = value;
            string[] args = value.Split(' ');
            string nonce = args[0];
            int victimID = System.Int32.Parse(args[1]);

            // set shooterID
            this.shooterID = this.shellShootProperties.OwnerID; 
            
            // set victimID
            this.victimID = victimID;
           
            // set initial position to where shooter is at
            this.transform.position = VRCPlayerApi
                .GetPlayerById(shooterID)
                .GetPosition();

            // set initialized flag to true
            this.initialized = true;

        }
    }

    void OnEnable()
    {

        this.doAfterSmokeAnimFinish = this
            .GetComponent<DoAfterSmokeAnimFinish>();
        this.returnShellCommand = this
            .GetComponent<ReturnShellCommand>();

        this.localPlayer = Networking.LocalPlayer;

        if (this.localPlayer.IsOwner(this.gameObject))
        {

            // if this is the shooter
            // set initialize fields and sync to other non-shooter instances
            this.Init = string.Join(
                " ",
                System.Guid.NewGuid().ToString().Substring(0, 6),
                ShellBehaviour.FindVictim(this.localPlayer.playerId).playerId
            );

        }

    }

    void FixedUpdate()
    {

        // if not yet initialized, do not run below
        if (!this.initialized) return;

        Vector3 victimPos = VRCPlayerApi
            .GetPlayerById(this.victimID)
            .GetPosition();

        float shellToVictimDist = Vector3.Distance(
            victimPos,
            this.transform.position
        );

        if (shellToVictimDist > .5f)
        {

            // rotate shell to face victim
            Vector3 direction = victimPos - this.transform.position;
            direction = direction.normalized;
            this.transform.forward = direction;

            // note: shell stops rotating after dist <= .5f
            // this is to prevent shell from rapidly rotating
            // when close to victim

        }

        // shell chases victim
        this.transform.position = Vector3.MoveTowards(
            this.transform.position,
            victimPos,
            this.shellProperties.Speed * Time.fixedDeltaTime
        );

    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {

        // if not yet initialized, do not run below
        if (!this.initialized) return;

        if (
            this.localPlayer.playerId == this.victimID
            && player.playerId == this.victimID
        )
        {

            // on victim's instance, they are hit by this shell
            // victim should be stunned
            // shell should be returned to shellPool

            // put victim to StunnedState
            this.leftController.SwitchState(
                this
                    .leftController
                    .TetherStatesDict
                    .StunnedState
                    .Initialize(5.0f)
            );
            this.rightController.SwitchState(
                this
                    .rightController
                    .TetherStatesDict
                    .StunnedState
                    .Initialize(5.0f)
            );

            // smoke particle system stops playing
            this.SendCustomNetworkEvent(
                VRC.Udon.Common.Interfaces.NetworkEventTarget.All,
                "StopSmoke"
            );

        }

    }

    public void BeforeReturnShell()
    {

        // SmokeAnimFinishCommand smokeAnimFinishCommand = this
        //     .GetComponent<SmokeAnimFinishCommand>().Init(this);

        // DoAfterSmokeAnimFinish doAfterSmokeAnimFinish = this
        //     .GetComponent<DoAfterSmokeAnimFinish>();

        // wait until all smoke particles are gone before returning shell

        this.doAfterSmokeAnimFinish.Initiate(
            this.smoke, 
            this.returnShellCommand.Init(this)
        );

    }

    public void ReturnShell()
    {
        this.ShellPool.Return(this.gameObject);
    }

    public void StopSmoke()
    {
        this.smoke.Stop();
        if (this.localPlayer.playerId == this.shooterID)
        {
            this.BeforeReturnShell();
        }
    }

    void OnDisable()
    {
        this.initialized = false;
    }

    public static VRCPlayerApi FindVictim(int shooterID)
    {

        // load all players into array
        VRCPlayerApi[] allPlayers = VRCPlayerApi.GetPlayers(
            new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()]
        );

        if (allPlayers.Length == 1)
        {
            return VRCPlayerApi.GetPlayerById(1);
        }

        VRCPlayerApi shooter = VRCPlayerApi
            .GetPlayerById(shooterID);

        VRCPlayerApi victim = null;

        // temporarily set victim to
        // 1st non-shooter player in the array
        int start = 0;
        if (allPlayers[start] == shooter)
        {
            if (VRCPlayerApi.GetPlayerCount() > 1)
            {
                ++start;
            }
        }
        victim = allPlayers[start];
        bool currentBestFacing = Vector3.Dot(
            shooter.GetRotation() * Vector3.forward,
            victim.GetPosition() - shooter.GetPosition()
        ) > 0;
        float currentBestDistance = Vector3.Distance(
            shooter.GetPosition(),
            victim.GetPosition()
        );
        ++start;

        // find closest player as our victim
        for (int i = start; i < allPlayers.Length; ++i)
        {

            if (allPlayers[i] != shooter)
            {

                // player being checked isn't shooter of shell
                // (so they can be potential victim)

                bool tempFacing = Vector3.Dot(
                    shooter.GetRotation() * Vector3.forward,
                    allPlayers[i].GetPosition() - shooter.GetPosition()
                ) > 0;

                float tempDistance = Vector3.Distance(
                    shooter.GetPosition(),
                    allPlayers[i].GetPosition()
                );

                if (tempFacing && !currentBestFacing)
                {
                    // player being checked is in front of shooter
                    // current best match is behind shooter
                    // update this player to be the victim and current best match
                    victim = allPlayers[i];
                    currentBestFacing = tempFacing;
                    currentBestDistance = tempDistance;
                }
                else if (tempFacing == currentBestFacing)
                {
                    // both players being checked and current best match
                    // are in front or behind the shooter
                    if (tempDistance < currentBestDistance)
                    {
                        // player being checked is closer to shooter
                        // update this player to be the victim and current best match
                        victim = allPlayers[i];
                        currentBestDistance = tempDistance;
                    }
                }

            }

        }

        return victim;

    }

}