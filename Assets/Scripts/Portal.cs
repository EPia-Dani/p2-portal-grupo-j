using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] public Portal otherPortal;
    [SerializeField] public Camera camera;
    [SerializeField] public Transform virtualPortal;
    [HideInInspector] public float scale;

    public void Update()
    {
        Vector3 i_position = virtualPortal.InverseTransformPoint(Camera.main.transform.position);
        otherPortal.camera.transform.position = otherPortal.transform.TransformPoint(i_position);

        Vector3 i_direction = virtualPortal.InverseTransformDirection(Camera.main.transform.forward);
        otherPortal.camera.transform.forward = otherPortal.transform.TransformDirection(i_direction);

        /*float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        camera.nearClipPlane = Mathf.Max(0.01f, distance);*/
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
