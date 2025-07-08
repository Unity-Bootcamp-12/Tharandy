using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Sensor : MonoBehaviour, IDetectable
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 360 * Time.deltaTime, 0));
    }

    public Vector3 Scan()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 100f))
        {
            Debug.DrawLine(origin, hit.point, Color.red);
            Debug.Log($"[Sensor] 감지됨: {hit.collider.name} at {hit.point}");
        }
        else
        {
            Debug.DrawRay(origin, direction * 100f, Color.green);
        }
        return hit.point;
    }
}
