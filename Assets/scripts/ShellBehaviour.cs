using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class ShellBehaviour : UdonSharpBehaviour
{

    [SerializeField]
    private VRCObjectPool shellPool;
    [SerializeField]
    private Tether.TetherController leftController;
    [SerializeField]
    private Tether.TetherController rightController;

    private int shooterID = -1;

    // SetShooterID = "arg[0] arg[1] arg[2]"
    // arg[0]: nonce
    // arg[1]: shooterID
    // arg[2]: callback
    [UdonSynced, FieldChangeCallback(nameof(SetShooterID))]
    private string setShooterID = "";
    public string SetShooterID
    {
        get => this.setShooterID;
        set
        {

            this.setShooterID = value;
            string[] args = value.Split();

            // this line here really sets shooterID
            this.shooterID = System.Int32.Parse(args[1]);

            if (args[2] != "")
            {
                // callback
                this.SendCustomEvent(args[2]);
            }

        }
    }
    
    [UdonSynced]
    private int victimID = -1;

    private VRCPlayerApi localPlayer;

    [SerializeField]
    private float speed = 1.5f;

    public void SetInitPos()
    {
        if (
            this.shooterID != -1
        )
        {
            Debug.Log("shooterID: " + this.shooterID);
            this.transform.position = VRCPlayerApi
                .GetPlayerById(this.shooterID)
                .GetPosition();
        }
    }

    void Start()
    {
        this.localPlayer = Networking.LocalPlayer;
    }

    public ShellBehaviour Init()
    {

        this.localPlayer = Networking.LocalPlayer;

        Networking.SetOwner(
            this.localPlayer,
            this.gameObject
        );

        this.SetShooterID = string.Join(
            " ",
            Random.Range(int.MinValue, int.MaxValue),
            this.localPlayer.playerId,
            "SetInitPos"
        );

        this.victimID = this.FindVictim().playerId;

        return this;

    }

    void FixedUpdate()
    {

        if (
            this.shooterID != -1
            && this.victimID != -1
        )
        {
            // valid shooter and victim
            this.transform.position = Vector3.MoveTowards(
                this.transform.position,
                VRCPlayerApi.GetPlayerById(this.victimID).GetPosition(),
                this.speed * Time.fixedDeltaTime
            );
        }

    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {

        if (
            this.localPlayer != null
            && this.localPlayer.playerId == this.victimID
            && player.playerId == this.victimID
        )
        {

            // on victim's instance, they are hit by this shell
            // victim should be stunned
            // shell should be returned to shellPool

            Networking.SetOwner(
                this.localPlayer,
                this.shellPool.gameObject
            );

            Networking.SetOwner(
                this.localPlayer,
                this.gameObject
            );

            // reset shooterID to -1
            this.SetShooterID = string.Join(
                " ",
                Random.Range(int.MinValue, int.MaxValue),
                -1,
                ""
            );

            // reset victimID to -1
            this.victimID = -1;

            // put victim to StunnedState
            this.leftController.SwitchState(
                this
                    .leftController
                    .TetherStatesDict
                    .StunnedState
                    .Initialize(3.0f)
            );
            this.rightController.SwitchState(
                this
                    .rightController
                    .TetherStatesDict
                    .StunnedState
                    .Initialize(3.0f)
            );

            // return shell to shellPool
            this.shellPool.Return(this.gameObject);

        }

    }

    private VRCPlayerApi FindVictim()
    {

        // load all players into array
        VRCPlayerApi[] allPlayers = VRCPlayerApi.GetPlayers(
            new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()]
        );

        VRCPlayerApi shooter = VRCPlayerApi
            .GetPlayerById(this.shooterID);

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