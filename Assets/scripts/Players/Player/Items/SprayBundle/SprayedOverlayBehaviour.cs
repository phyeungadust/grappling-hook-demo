using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class SprayedOverlayBehaviour : UdonSharpBehaviour
{

    private Color sprayedColor;

    [SerializeField]
    private float defaultTimeBeforeClear = 3.0f;
    private float timeBeforeClear;
    [SerializeField]
    private float sprayAccumulateSpeed = 2.0f;
    // [SerializeField]
    // private float sprayClearSpeed = 2.0f;
    [SerializeField]
    private MeshRenderer mesh;
    private Material sprayedOverlayMaterial;

    [SerializeField]
    private Light lightComp;
    private bool hasSpray = false;

    [SerializeField]
    private PlayerStore ownerStore;
    private Transform headFollower;

    public void CustomStart()
    {
        this.headFollower = this.ownerStore.follower.head;
        this.sprayedOverlayMaterial = this.mesh.material;
        this.sprayedColor = this.sprayedOverlayMaterial.color;
    }

    public void CustomUpdate()
    {

        this.transform.SetPositionAndRotation(
            this.headFollower.position,
            this.headFollower.rotation
        );

        if (this.hasSpray)
        {
            if (this.timeBeforeClear <= 0.0f)
            // there is still spray on screen, clear now
            this.ClearSpray();
        }

    }

    public void CustomFixedUpdate()
    {
        if (this.timeBeforeClear > 0.0f)
        {
            this.timeBeforeClear -= Time.fixedDeltaTime;
        }
    }

    public void SprayScreenLocal()
    {
        // increase alpha (amount of spray on the screen)
        // not more than 1.0f

        if (!this.hasSpray)
        {
            // spray just started
            // enable mesh and lightComp
            this.mesh.enabled = true;
            this.lightComp.enabled = true;
            this.hasSpray = true;
        }

        this.sprayedColor.a = Mathf.Min(
            this.sprayedOverlayMaterial.color.a 
                + this.sprayAccumulateSpeed * Time.deltaTime,
            1.0f
        );

        this.sprayedOverlayMaterial.color = this.sprayedColor;

        this.timeBeforeClear = this.defaultTimeBeforeClear;

    }

    private void ClearSpray()
    {

        // decrease alpha (amount of spray on the screen)
        // not less than 0.0f
        this.sprayedColor.a = Mathf.Max(
            this.sprayedColor.a 
                - this.sprayAccumulateSpeed * Time.deltaTime,
            0.0f
        );

        this.sprayedOverlayMaterial.color = this.sprayedColor;

        if (Mathf.Abs(this.sprayedColor.a - 0.0f) <= 1e-4)
        {
            // if alpha drops to 0, 
            // simply disable mesh and lightComp
            // and mark as "no spray"
            this.mesh.enabled = false;
            this.lightComp.enabled = false;
            this.hasSpray = false;
        }

    }

}