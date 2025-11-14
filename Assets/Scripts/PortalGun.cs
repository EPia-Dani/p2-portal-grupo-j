using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    [SerializeField] private GameObject bluePortalPrefab;
    [SerializeField] private GameObject orangePortalPrefab;
    public static Portal bluePortal;
    public static Portal orangePortal;
    [SerializeField] private List<Transform> validPoints;
    private float m_ValidPointOffset = 0.03f;
    private float m_MaxValidAngle = 5.0f;

    [SerializeField] private Transform attachPosition;

    [SerializeField] private float distance;
    [SerializeField] private float gravityGunForce;
    private Rigidbody attachedObject;
    private Quaternion attachedRotation;
    private bool hasObjectAttached;
    [SerializeField] private float attachingSpeed;

    private InputController input;

    void Start()
    {
        input = GetComponent<InputController>();
    }

    void Update()
    {
        if (attachedObject)
        {
            UpdateAttachedObject();
            if (input.button1)
            {
                ThrowObject(gravityGunForce);
                input.button1 = false;
            }
            if (input.button2)
            {
                ThrowObject(0);
                input.button2 = false;
            }
        }
        else
        {
            if (input.button1 || input.button2)
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                Physics.Raycast(ray, out var hit, distance);
                if (hit.collider.CompareTag("Turret") || hit.collider.CompareTag("Cube"))
                {
                    AttachObject(hit.rigidbody);
                    /*input.button1 = false;
                    input.button2 = false;
                    return;*/
                }

                if (hit.collider.CompareTag("Printable"))
                {
                    if (input.button1) CreateBluePortal(hit);
                    if (input.button2) CreateOrangePortal(hit);
                }

                input.button1 = false;
                input.button2 = false;
            }
        }
        
    }    

    void UpdateAttachedObject()
    {
        Vector3 eulerAngles = attachPosition.rotation.eulerAngles;
        if (!hasObjectAttached)
        {
            Vector3 direction = attachPosition.transform.position - attachedObject.transform.position;
            float distance = direction.magnitude;
            float movement = attachingSpeed * Time.deltaTime;
            if (movement >= distance)
            {
                hasObjectAttached = true;
                attachedObject.MovePosition(attachPosition.position);
                attachedObject.MoveRotation(Quaternion.Euler(0.0f, eulerAngles.y, eulerAngles.z));
            }
            else
            {
                direction /= distance;
                attachedObject.MovePosition(attachedObject.transform.position + direction * movement);
                attachedObject.MoveRotation(Quaternion.Lerp(attachedRotation, Quaternion.Euler(0.0f, eulerAngles.y, eulerAngles.z), 1.0f - Mathf.Min(distance / 1.5f, 1.0f)));
            }
        }
        else
        {
            attachedObject.MoveRotation(Quaternion.Euler(0.0f, eulerAngles.y, eulerAngles.z));
            attachedObject.MovePosition(attachPosition.position);
        }
    }

    private void AttachObject(Rigidbody rb)
    {
        attachedObject = rb;
        attachedRotation = rb.rotation;
        attachedObject.isKinematic = true;
        hasObjectAttached = false;

        CompanionCube companionCube = rb.GetComponent<CompanionCube>();
        if (companionCube != null)
            companionCube.SetTeleportable(false);
    }

    private void ThrowObject(float force)
    {
        attachedObject.isKinematic = false;
        attachedObject.AddForce(attachPosition.forward * force, ForceMode.Impulse);

        CompanionCube companionCube = attachedObject.GetComponent<CompanionCube>();
        if (companionCube != null)
            companionCube.SetTeleportable(true);
        attachedObject = null;
    }

    public void CreateBluePortal(RaycastHit hit)
    {
        if (bluePortal != null) bluePortal.Close();
        GameObject portalGameObject = Instantiate(bluePortalPrefab, hit.point + hit.normal.normalized * .1f, Quaternion.LookRotation(hit.normal));
        bluePortal = portalGameObject.GetComponent<Portal>();
        if (orangePortal == null) return;
        bluePortal.otherPortal = orangePortal;
        orangePortal.otherPortal = bluePortal;
    }

    public void CreateOrangePortal(RaycastHit hit)
    {
        if (orangePortal != null) orangePortal.Close();
        GameObject portalGameObject = Instantiate(orangePortalPrefab, hit.point + hit.normal.normalized * .1f, Quaternion.LookRotation(hit.normal));
        orangePortal = portalGameObject.GetComponent<Portal>();
        if (bluePortal == null) return;
        bluePortal.otherPortal = orangePortal;
        orangePortal.otherPortal = bluePortal;
    }


    public bool IsValidPoint(Vector3 position, Vector3 normal)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(normal);

        bool isValid = true;
        Vector3 playerCameraPos = Camera.main.transform.position;

        /*
        for (int i = 0; i < validPoints.Count; i++)
        {
            Vector3 l_direction = validPoints[i].position - playerCameraPos;
            float l_distance = l_direction.magnitude;
            l_direction /= l_distance;

            Ray l_ray = new Ray(playerCameraPos, l_direction);

            if (Physics.Raycast(l_ray, out RaycastHit l_hit, l_distance + m_ValidPointOffset, m_ValidLayerMask.value))
            {
                if (l_hit.collider.CompareTag("Printable"))
                {
                    float l_distanceToHitPoint = (l_hit.point - validPoints[i].position).magnitude;

                    if (l_distanceToHitPoint < m_ValidPointOffset)
                    {
                        float l_dotAngle = Vector3.Dot(l_hit.normal, normal);
                        if (l_dotAngle < Mathf.Cos(m_MaxValidAngle * Mathf.Deg2Rad))
                        {
                            isValid = false;
                        }
                    }
                    else
                    {
                        isValid = false;
                    }
                }
                else
                {
                    isValid = false;
                }
            }
            else
            {
                isValid = false;
            }
        }
        */
        return isValid;
        
    }
}
