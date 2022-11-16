using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject _impactPrefab;

    public Vector3 Direction;
    public LayerMask IgnoredLayers;

    [SerializeField] private float _speed;
    [SerializeField] private float _distanceCheck;

    private void Update()
    {
        Ray projectileRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(projectileRay, out RaycastHit hit, _distanceCheck, ~IgnoredLayers))
        {
            GameObject.Instantiate(_impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject.Destroy(gameObject);
        }

        transform.position += Direction * _speed * Time.deltaTime;
    }
}
