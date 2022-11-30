using Assets.CodeBase.Infrastructure;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public static float PlayerSpeed;

    [SerializeField] private Transform _gunPoint;
    [SerializeField] private LayerMask _ignoredLayers;
    [SerializeField] private float _randomSpread;
    [SerializeField] private int _shootDelay;

    private AllServices _services = AllServices.Instance;

    private InputService _inputService;
    private int _shootPrepair;


    private const float RayDistance = 1000f;

    private void Start()
    {
        _inputService = _services.GetService<InputService>();

    }

    private void Update()
    {
        _shootPrepair++;
        if (_inputService.GetShootButton() && _shootPrepair >= _shootDelay)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(_inputService.MousePosition);

        if (Physics.Raycast(cameraRay, out RaycastHit hit, RayDistance, ~_ignoredLayers) == true)
        {

            GameObject projectile = GameObject.Instantiate(ProjectilePrefab, _gunPoint.position, Quaternion.identity);
            projectile.transform.LookAt(hit.point);
            projectile.transform.rotation *= RandomSpread();
            _shootPrepair = 0;
        }
    }

    private Quaternion RandomSpread()
    {
        float spreadMult = 1 + PlayerSpeed; 
        return Quaternion.Euler(Random.Range(-_randomSpread, _randomSpread + 1) * spreadMult, Random.Range(-_randomSpread, _randomSpread + 1) * spreadMult, Random.Range(-_randomSpread, _randomSpread + 1) * spreadMult);
    }
}
