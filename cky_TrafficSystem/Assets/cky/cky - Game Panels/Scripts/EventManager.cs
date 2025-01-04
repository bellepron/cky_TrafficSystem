using System;

namespace cky.GamePanels
{
    public class EventManager
    {
        private static EventManager _instance;
        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventManager();
                }

                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        private EventManager() { }

        public static event Action OnGameStart, OnGameSuccess, OnGameFail;

        public void Initialize()
        {
            OnGameStart = null;
            OnGameSuccess = null;
            OnGameFail = null;
        }

        public void Add_OnGameStart(Action x) => OnGameStart += x;
        public void Remove_OnGameStart(Action x) => OnGameStart -= x;
        public void Add_OnGameSuccess(Action x) => OnGameSuccess += x;
        public void Remove_OnGameSuccess(Action x) => OnGameSuccess -= x;
        public void Add_OnGameFail(Action x) => OnGameFail += x;
        public void Remove_OnGameFail(Action x) => OnGameFail -= x;

        public void GameStartEvent() => OnGameStart?.Invoke();
        public void GameSuccessEvent() => OnGameSuccess?.Invoke();
        public void GameFailEvent() => OnGameFail?.Invoke();
    }
}