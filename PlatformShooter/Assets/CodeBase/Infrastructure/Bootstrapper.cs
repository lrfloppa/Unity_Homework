using UnityEngine;

namespace Assets.CodeBase.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GameObject _heroPrefab;

        private void Start()
        {
            InitializeServices();

            GameStateMachine game = new GameStateMachine(AllServices.Instance.GetService<FactoryHero>());
            game.SwitchState<BootstrapperState>();
        }

        private void InitializeServices()
        {
            AllServices.Instance.RegisterService<FactoryHero>(new FactoryHero(_heroPrefab));
            AllServices.Instance.RegisterService<InputService>(new InputService());
        }
    }
}
