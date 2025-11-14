using UnityEngine;

public class CompanionCube : MonoBehaviour, ITeleportable
{
    private bool canTeleport;
    private bool isTeleporting = false;
    private Rigidbody rb;

    public void SetTeleportable(bool teleportable)
    {
        canTeleport = teleportable;
        rb = GetComponent<Rigidbody>();
    }


    public void Teleport(Portal portal)
    {
        if (isTeleporting || !canTeleport) return;

        isTeleporting = true;
        Vector3 position = portal.virtualPortal.InverseTransformPoint(transform.position);
        Vector3 direction = portal.virtualPortal.InverseTransformDirection(transform.forward);

        transform.position = portal.otherPortal.transform.TransformPoint(position);
        transform.forward = portal.otherPortal.transform.TransformDirection(direction);

        Vector3 velocity = portal.virtualPortal.InverseTransformDirection(rb.linearVelocity);
        rb.linearVelocity = portal.otherPortal.transform.TransformDirection(velocity);
    }

    public void EndTeleport()
    {
        isTeleporting = false;
    }
}
