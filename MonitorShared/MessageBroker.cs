#region Descripción
/*
    Implementa un gestor de mensajes con sincronización de contexto.
*/
#endregion


#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
#endregion


namespace Monitor.Shared
{
    /// <summary>
    ///  Implementa un gestor de mensajes. Los receptores se suscriben para recibir. 
    ///  Los mensajes se envian de forma asíncrona al contexto de sincronización.
    /// </summary>
    internal class MessageBroker
    {
        private readonly Dictionary<Type, object> consumers;
        private readonly SynchronizationContext synchC;


        public MessageBroker()
        {
            consumers = new Dictionary<Type, object>();
            synchC = AsyncOperationManager.SynchronizationContext;
        }


        public void Send<T>(T message)
        {
            var key = typeof(T);
            if (!consumers.ContainsKey(key)) return;

            var list = consumers[key] as List<Action<T>>;
            list.ForEach(action =>
                       synchC.Post(m => action((T)m), message));

        }

        public void Register<T>(Action<T> action)
        {
            var key = typeof(T);
            List<Action<T>> list;

            if (!consumers.ContainsKey(key))
            {
                list = new List<Action<T>>();
                consumers.Add(key, list);
            }
            else
                list = consumers[key] as List<Action<T>>;

            list.Add(action);
        }

        public void Remove<T>(Action<T> action)
        {
            var key = typeof(T);

            if (consumers.ContainsKey(key))
            {
                List<Action<T>> list = consumers[key] as List<Action<T>>;
                if (list.Contains(action))
                {
                    list.Remove(action);
                }
            }
        }
    }
    public static class MessageBus
    {
        private static MessageBroker messageBroker;

        // Singleton creation
        private static MessageBroker Broker
        {
            get
            {
                if (messageBroker == null)
                    messageBroker = new MessageBroker();

                return messageBroker;
            }
        }

        public static void Register<T>(Action<T> action)
        {
            Broker.Register(action);
        }

        public static void Remove<T>(Action<T> action)
        {
            Broker.Remove(action);
        }

        public static void Send<T>(T message)
        {
            Broker.Send(message);
        }
    }
}
