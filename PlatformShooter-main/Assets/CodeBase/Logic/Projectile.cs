using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public Vector3 Direction;
    public LayerMask IgnoredLayers;

    [SerializeField] private GameObject _particlePrefab;
    [SerializeField] private float _distanceCheck = 0.15f;
    [SerializeField] private float _speed;

    private void Update()
    {
        Ray projectileRay = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(projectileRay, out RaycastHit hit, _distanceCheck, ~IgnoredLayers) == true)
        {
            GameObject.Instantiate(_particlePrefab, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject.Destroy(gameObject);
        }

        transform.position += Direction * _speed * Time.deltaTime;
    }
}
