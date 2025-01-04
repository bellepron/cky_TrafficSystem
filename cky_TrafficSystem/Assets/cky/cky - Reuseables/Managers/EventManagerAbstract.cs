using cky.Reuseables.Singleton;
using System;

namespace cky.Reuseables.Managers
{
    public abstract class EventManagerAbstract<T>
    {
        public static event Action GameEnd, GameSuccess, GameFail;

        #region Core
        public void GameEndEvent() => GameEnd?.Invoke();
        public void GameSuccessEvent() => GameSuccess?.Invoke();
        public void GameFailEvent() => GameFail?.Invoke();

        #endregion
    }
}