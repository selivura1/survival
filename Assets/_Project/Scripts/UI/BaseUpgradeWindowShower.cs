using Selivura.Player;
using UnityEngine;

namespace Selivura.UI
{
    public class BaseUpgradeWindowShower : MonoBehaviour, IInteractable
    {
        [SerializeField] ShopWindow _prefab;
        [SerializeField] MainBase _mainBase;
        [SerializeField] Sprite _baseUpgradeIcon;
        [SerializeField] Vector3 _offset = new Vector3(0, 1);
        ShopWindow _current;
        Timer _timer = new Timer(0, 0);
        [SerializeField] float _hideWindowTime = 2;

        [Inject]
        OverlayWindowsContainer _overlayWindowsContainer;
        private void Awake()
        {
            FindFirstObjectByType<Injector>().Inject(this);
        }
        public bool CanInteract(PlayerUnit interactor)
        {
            if (!_current)
            {
                _current = Instantiate(_prefab, _overlayWindowsContainer.transform);
                _current.Initialize("Upgrade base", "Increases base health and energy regeneration range", _mainBase.XPToLevelUp, _baseUpgradeIcon);
                _current.gameObject.AddComponent<PositionLocker>().Initialize(transform.position + _offset);
            }
            _timer = new Timer(_hideWindowTime, Time.time);
            return false;
        }
        private void DespawnWindow()
        {
            _current.HideWindow();
            _current = null;
        }
        private void FixedUpdate()
        {
            if (_timer.Expired && _current != null)
            {
                DespawnWindow();
            }
        }
        private void OnDisable()
        {
            if (_current != null)
                DespawnWindow();
        }
        public void Interact(PlayerUnit interactor)
        {
            if (_mainBase != null)
                if (!_mainBase.CanInteract(interactor))
                {
                    if (_current)
                        _current.DoShakeAnimation();
                }
        }

        public string GetInteractionName()
        {
            return "";
        }
    }
}
