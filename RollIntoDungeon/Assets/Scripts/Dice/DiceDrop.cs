using UnityEngine;
using UnityEngine.InputSystem;

public class DiceDrop : MonoBehaviour
{
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float maxTorque = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Drop();
        }
    }

    void Drop()
    {
        transform.position = dropPoint.position;
        transform.rotation = Random.rotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 randomTorque = new Vector3(
            Random.Range(-maxTorque, maxTorque),
            Random.Range(-maxTorque, maxTorque),
            Random.Range(-maxTorque, maxTorque));

        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }
}
