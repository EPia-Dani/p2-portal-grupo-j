using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] public Portal otherPortal;
    [SerializeField] public Camera camera;
    [SerializeField] public Transform virtualPortal;
    [HideInInspector] public float size;

    public void Update()
    {
        Vector3 position = virtualPortal.InverseTransformPoint(Camera.main.transform.position);
        otherPortal.camera.transform.position = otherPortal.transform.TransformPoint(position);

        Vector3 direction = virtualPortal.InverseTransformDirection(Camera.main.transform.forward);
        otherPortal.camera.transform.forward = otherPortal.transform.TransformDirection(direction);

        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        otherPortal.camera.nearClipPlane = Mathf.Max(0.01f, distance);
    }

    public void SetSize(float size)
    {
        this.size = size;
        transform.localScale = new Vector3(size, size, 1);
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        ITeleportable teleportable = other.gameObject.GetComponent<ITeleportable>();
        if (teleportable != null)
        {
            teleportable.Teleport(this);
        }
    }


    public void OnTriggerExit(Collider other)
    {
        ITeleportable teleportable = other.gameObject.GetComponent<ITeleportable>();
        if (teleportable != null)
        {
            teleportable.EndTeleport();
        }
    }
}
