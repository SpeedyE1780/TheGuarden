namespace TheGuarden.Utility
{
    /// <summary>
    /// Interface indicating that class can be added to a pool
    /// </summary>
    public interface IPoolObject
    {
        /// <summary>
        /// Reset state before going in pool
        /// </summary>
        void OnEnterPool();
        /// <summary>
        /// Initialize state before leaving pool
        /// </summary>
        void OnExitPool();
    }
}
