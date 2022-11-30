using UnityEngine;

namespace Assets.CodeBase.Infrastructure
{
    public class FactoryHero : IService
    {
        private readonly GameObject _heroPrefab;

        public FactoryHero(GameObject hero)
        {
            _heroPrefab = hero;
        }

        public GameObject BuildHero(Vector3 at)
            => GameObject.Instantiate(_heroPrefab, at, Quaternion.identity);
    }
}
