using UnityEngine;
using Fusion;

public class VRPlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject leftHandController;
    [SerializeField] private GameObject rightHandController;
    [SerializeField] private GameObject headset;
    [SerializeField] private Camera playerCamera;

    private void Start()
    {
        if (HasStateAuthority)
        {
            // Assign input and camera to the local player only
            InitializeLocalPlayer();
        }
        else
        {
            // Disable other players' cameras and controllers
            playerCamera.enabled = false;
            leftHandController.SetActive(false);
            rightHandController.SetActive(false);
            headset.SetActive(false);
        }
    }

    private void InitializeLocalPlayer()
    {
        // Ensure the local player's camera is enabled
        playerCamera.enabled = true;

        // Ensure the local player's controllers and headset are active
        leftHandController.SetActive(true);
        rightHandController.SetActive(true);
        headset.SetActive(true);
    }

    private void Update()
    {
        if (!HasStateAuthority)
        {
            return;
        }

        // Handle local player input (if needed)
        HandleInput();
    }

    private void HandleInput()
    {
        // Example: Update the transforms of the local player's controllers and headset
        // Replace with actual input handling logic

        // Simulate updating positions and rotations (replace with actual VR input handling)
        Vector3 leftHandPosition = leftHandController.transform.position;
        Quaternion leftHandRotation = leftHandController.transform.rotation;
        Vector3 rightHandPosition = rightHandController.transform.position;
        Quaternion rightHandRotation = rightHandController.transform.rotation;
        Vector3 headsetPosition = headset.transform.position;
        Quaternion headsetRotation = headset.transform.rotation;

        // Send input data to the server if needed
        RPC_UpdateTransforms(leftHandPosition, leftHandRotation, rightHandPosition, rightHandRotation, headsetPosition, headsetRotation);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_UpdateTransforms(Vector3 leftHandPosition, Quaternion leftHandRotation, Vector3 rightHandPosition, Quaternion rightHandRotation, Vector3 headsetPosition, Quaternion headsetRotation)
    {
        if (HasStateAuthority)
        {
            // Apply the transforms on the authoritative side
            leftHandController.transform.SetPositionAndRotation(leftHandPosition, leftHandRotation);
            rightHandController.transform.SetPositionAndRotation(rightHandPosition, rightHandRotation);
            headset.transform.SetPositionAndRotation(headsetPosition, headsetRotation);
        }
    }
}