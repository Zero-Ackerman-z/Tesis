using UnityEngine;

using UnityEngine.InputSystem;

public class Activateteleportationray : MonoBehaviour
{

    public GameObject leftTeleportation;
    public GameObject rightTeleportation;

    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

    public InputActionProperty leftCancel;
    public InputActionProperty rightCancel;

    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor leftRay;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rightRay;

    void Update()
    {
        bool isLeftRayHovering = leftRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftNumber, out bool leftValid);
        leftTeleportation.SetActive(!isLeftRayHovering && leftCancel.action.ReadValue<float>() == 0 && leftActivate.action.ReadValue<float>() > 0.1f);

        bool isRightRayHovering = leftRay.TryGetHitInfo(out Vector3 RightPos, out Vector3 RightNormal, out int RightNumber, out bool RightValid);
        rightTeleportation.SetActive(!isRightRayHovering && rightCancel.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>() > 0.1f);

    }
}
