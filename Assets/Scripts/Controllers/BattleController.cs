using Cards;
using UnityEngine;

namespace Controllers
{
    public class BattleController : EventController
    {
        private Camera _mainCamera;

        protected override void Start()
        {
            base.Start();
            _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            
            _mainCamera.gameObject.transform.rotation = new Quaternion(0, -10, 0, 0);
        }

        public override void Show(LevelCard card)
        {
            base.Show(card);
            //_mainCamera.gameObject.transform.rotation = new Quaternion(0, -10, 0, 0);
        }
    }
}