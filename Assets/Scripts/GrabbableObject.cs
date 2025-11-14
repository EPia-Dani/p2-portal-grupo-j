using UnityEngine;

public abstract class GrabbableObject : MonoBehaviour
{
    protected Rigidbody rb;

    protected bool isGrabbed;
    protected Transform attachedPosition;
    protected float attachSpeed = 10;
    protected Quaternion rotationWhenAttached;

    protected void UpdateGrabbableObject()
    {
        Vector3 eulerAngles = attachedPosition.rotation.eulerAngles;
        if (!isGrabbed)
        {
            Vector3 direction = attachedPosition.transform.position - rb.transform.position;
            float distance = direction.magnitude;
            float movement = attachSpeed * Time.deltaTime;
            if (movement >= distance)
            {
                isGrabbed = true;
                rb.MovePosition(attachedPosition.position);
                rb.MoveRotation(Quaternion.Euler(0.0f, eulerAngles.y, eulerAngles.z));
            }
            else
            {
                direction /= distance;
                rb.MovePosition(rb.transform.position + direction * movement);
                rb.MoveRotation(Quaternion.Lerp(rotationWhenAttached, Quaternion.Euler(0.0f, eulerAngles.y, eulerAngles.z), 1.0f - Mathf.Min(distance / 1.5f, 1.0f)));
            }
        }
        else
        {
            rb.MoveRotation(Quaternion.Euler(0.0f, eulerAngles.y, eulerAngles.z));
            rb.MovePosition(attachedPosition.position);
        }
    }

    public void OnGrab(Transform attachedPosition)
    {
        rb.isKinematic = true;
        this.attachedPosition = attachedPosition;
        rotationWhenAttached = transform.rotation;
    }

    public void OnThrow(float force)
    {
        rb.isKinematic = false;
        attachedPosition = null;
    }
}
