using UnityEngine;

namespace Sokabon.CommandSystem
{
    public class PutGel : Command
    {
        private readonly GameObject _gelPrefab;
        private readonly Player _player;
        private GameObject _instantiatedGel;

        public PutGel(GameObject gelPrefab, Player player)
        {
            _gelPrefab = gelPrefab;
            _player = player;
            IsPlayerInput = true;
        }
        
        public override void Execute(System.Action onComplete)
        {
            if (_player._gelCount <= 0)
            {
                Debug.Log("No gel to put");
                onComplete?.Invoke();
                return;
            }
            ;
            _instantiatedGel = Object.Instantiate(_gelPrefab, _player.transform.position, Quaternion.identity);
            _player._gelCount--;
            onComplete?.Invoke();
        }
        
        public override void Undo(System.Action onComplete)
        {
            Object.Destroy(_instantiatedGel);
            _player._gelCount++;
            onComplete?.Invoke();
        }
    }
}