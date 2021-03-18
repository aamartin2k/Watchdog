#region Descripción
/*
    Implementa una cola FIFO de tamaño fijo que almacena elementos genericos.
*/
#endregion

#region Using
using System;
using System.Collections.Generic;
#endregion

namespace Monitor.Shared
{
    /// <summary>
    /// Implementa una cola FIFO de tamaño fijo que almacena elementos genericos. Al
    /// alcanzar el limite, los elementos mas antiguos se descartan.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class FixedSizedQueue<T> 
    {
 
        Queue<T> queue;

        public int MaxSize { get; set; }

        public FixedSizedQueue(int maxSize)
        {
            MaxSize = maxSize;
            queue = new Queue<T>();
        }

        public FixedSizedQueue(int maxSize, IEnumerable<T> items) 
        {
            MaxSize = maxSize;
         if (items == null) {
            queue = new Queue<T>();
         }
         else {
            queue = new Queue<T>(items);
            EnsureLimitConstraint();
         }
        }

        public void Enqueue(T obj) {
          queue.Enqueue(obj);
          EnsureLimitConstraint();
        }

        private void EnsureLimitConstraint() {
            if (queue.Count > MaxSize ) {
            
                 while (queue.Count > MaxSize ) {
                    queue.Dequeue();

               }
             }
        }

        /// <summary>
        /// Returns the current snapshot of the queue in an array[T].
        /// </summary>
        /// <returns></returns>
        public T[] GetSnapshot() 
        {
            EnsureLimitConstraint();
            return queue.ToArray();

        }

}
}
