using System;

namespace Core
{
    /// <summary>
    /// Controls the events in the game.
    /// </summary>
    public static class EventHandler
    {
        /// <summary>
        /// Called every time that one coins change its value.
        /// </summary>
        public static Action<int> OnCoinUpdate;
        public static void DispatchCoinUpdate(int coin) => OnCoinUpdate?.Invoke(coin);
    }
}