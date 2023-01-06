using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UIElements;

public class SprayedOverlayBehaviour : UdonSharpBehaviour
{

    private Color sprayedColor;
    private VRCPlayerApi localPlayer;

    [SerializeField]
    private float defaultTimeBeforeClear = 3.0f;
    private float timeBeforeClear;
    [SerializeField]
    private float sprayAccumulateSpeed = 2.0f;
    [SerializeField]
    private float sprayClearSpeed = 2.0f;
    private Material sprayedOverlayMaterial;

    void Start()
    {
        this.sprayedOverlayMaterial = this
            .GetComponent<MeshRenderer>()
            .material;
        this.sprayedColor = this.sprayedOverlayMaterial.color;
        this.localPlayer = Networking.LocalPlayer;
    }

    void Update()
    {

        // make cube mesh follow and cover player's head
        // to give overlay effect
        VRCPlayerApi.TrackingData td = this
            .localPlayer
            .GetTrackingData(
                VRCPlayerApi.TrackingDataType.Head
            );
        this.transform.SetPositionAndRotation(
            td.position,
            td.rotation
        );

        if (Input.GetKey(KeyCode.R))
        {
            this.SprayScreen();
        }

        if (this.timeBeforeClear <= 0.0f)
        {
            if (this.sprayedColor.a > 0.0f)
            {
                // there is still spray on screen, clear now
                this.ClearSpray();
            }
        }

    }

    void FixedUpdate()
    {
        if (this.timeBeforeClear > 0.0f)
        {
            this.timeBeforeClear -= Time.fixedDeltaTime;
        }
    }

    private void SprayScreen()
    {
        // increase alpha (amount of spray on the screen)
        // not more than 1.0f

        this.sprayedColor.a = Mathf.Min(
            this.sprayedOverlayMaterial.color.a 
                + this.sprayAccumulateSpeed * Time.deltaTime,
            0.8f
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

    }

}