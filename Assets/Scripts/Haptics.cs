using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Haptics : MonoBehaviour {
    public InputActionProperty hapticAction; // Reference to the Input Action for haptics

    private XRController xrController;

    private void Start() {
        xrController = GetComponent<XRController>();
    }

    public void TriggerHaptic(float amplitude, float duration) {
        if (xrController != null && xrController.SendHapticImpulse(amplitude, duration)) {
            Debug.Log("Haptic feedback triggered.");
        } else {
            Debug.LogWarning("Haptic feedback not supported.");
        }
    }

    // Example usage
    private void Update() {
        if (hapticAction.action.triggered) {
            TriggerHaptic(0.5f, 1.0f); // Trigger haptic feedback with amplitude 0.5 and duration 1 second
        }
    }
}