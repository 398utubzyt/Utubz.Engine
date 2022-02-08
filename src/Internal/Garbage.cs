using System.Collections.Generic;

namespace Utubz.Internal
{
    /// <summary>
    /// Handles automatic garbage collection of <see cref="Object"/>s.
    /// </summary>
    public static class Garbage
    {
        private static Queue<Object> gQueue;

        internal static void Init()
        {
            gQueue = new Queue<Object>();
        }

        internal static void Quit()
        {
            Process();
        }

        /// <summary>
        /// Removes the queued garbage from memory.
        /// </summary>
        public static void Process()
        {
            while (gQueue.Count > 0)
                gQueue.Dequeue().DestroyNow();
        }

        /// <summary>
        /// Registers an <see cref="Object"/> for garbage collection. This cannot be reversed.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to register.</param>
        public static void Register(Object obj)
        {
            if (gQueue.Contains(obj))
                return;

            gQueue.Enqueue(obj);
        }
    }
}
