using UnityEngine;

public abstract class GrabbableObject : MonoBehaviour
{
    private Rigidbody rigidbody;

    protected bool isGrabbed;
    protected Transform attachedPosition;
    protected float attachSpeed = 10;
    protected Quaternion rotationWhenAttached;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        Vector3 eulerAngles = attachedPosition.rotation.eulerAngles;
        if (!isGrabbed)
        {
            Vector3 direction = attachedPosition.transform.position - rigidbody.transform.position;
            float distance = direction.magnitude;
            float movement = attachSpeed * Time.deltaTime;
            if (movement >= distance)
            {
                isGrabbed = true;
                rigidbody.MovePosition(attachedPosition.position);
                rigidbody.MoveRotation(Quaternion.Euler(0.0f, eulerAngles.y, eulerAngles.z));
            }
            else
            {
                direction /= distance;
                rigidbody.MovePosition(rigidbody.transform.position + direction * movement);
                rigidbody.MoveRotation(Quaternion.Lerp(rotationWhenAttached, Quaternion.Euler(0.0f, eulerAngles.y, eulerAngles.z), 1.0f - Mathf.Min(distance / 1.5f, 1.0f)));
            }
        }
        else
        {
            rigidbody.MoveRotation(Quaternion.Euler(0.0f, eulerAngles.y, eulerAngles.z));
            rigidbody.MovePosition(attachedPosition.position);
        }
    }

    public void OnGrab(Transform attachedPosition)
    {
        rigidbody.useGravity = false;
        this.attachedPosition = attachedPosition;
        rotationWhenAttached = transform.rotation;
    }

    public void OnDrop()
    {
        OnThrow(0);
    }

    public void OnThrow(float force)
    {
        rigidbody.useGravity = true;
        attachedPosition = null;
    }
}
