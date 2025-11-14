using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] public Portal otherPortal;
    [SerializeField] public Camera camera;
    [SerializeField] public Transform virtualPortal;
    [HideInInspector] public float size;
    [SerializeField] private List<Transform> validPoints;
    private float validPointOffset = 0.2f;
    [SerializeField] private LayerMask printableLayer;

    public void Update()
    {
        if (otherPortal == null) return;
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

    public bool IsValidPoint()
    {
        bool isValid = true;
        Vector3 playerCameraPos = Camera.main.transform.position;

        for (int i = 0; i < validPoints.Count; i++)
        {
            Vector3 direction = validPoints[i].position - playerCameraPos;
            float distance = direction.magnitude;
            direction /= distance;

            Ray ray = new Ray(playerCameraPos, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, distance + validPointOffset, printableLayer))
            {
                if (!hit.collider.CompareTag("Printable"))
                {
                    isValid = false;
                }
            }
            else
            {
                isValid = false;
            }
        }
        return isValid;
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
