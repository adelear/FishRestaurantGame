using UnityEngine;

public class DirectionalLightRotator : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void Update()
    {
        float rotationAmount = Time.deltaTime * rotationSpeed;
        transform.Rotate(Vector3.right, rotationAmount);
    }
}
