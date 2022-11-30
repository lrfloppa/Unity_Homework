using UnityEngine;

namespace Assets.CodeBase.Infrastructure
{
    public class LoadLevelState : IState
    {
        private readonly FactoryHero _factoryHero;

        public LoadLevelState(FactoryHero factoryHero)
        {
            _factoryHero = factoryHero;
        }

        public void Enter()
        {
            CreateHeroAndCamera();

        }

        private void CreateHeroAndCamera()
        {
            Vector3 initialHeroSpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
            Transform cameraTarget = _factoryHero.BuildHero(initialHeroSpawnPoint).transform;

            Camera.main.transform.GetComponent<CameraFollow>().SetTarget(cameraTarget);
        }

        public void Exit()
        {
        }
    }
}
