using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Potency.Services.Runtime.Tick
{
    public class TickService : ITickService, IDisposable
    {
        private readonly TickServiceMonoComponent _tickMonoComponent;

        private readonly List<TickData> _onUpdateList = new();
        private readonly List<TickData> _onFixedUpdateList = new();
        private readonly List<TickData> _onLateUpdateList = new();

        private int _tickDataIdRef;

        public TickService()
        {
            var tickObject = new GameObject("TickServiceMonoComponent");
        
            Object.DontDestroyOnLoad(tickObject);

            _tickMonoComponent = tickObject.AddComponent<TickServiceMonoComponent>();
            _tickMonoComponent.OnUpdate = OnUpdate;
            _tickMonoComponent.OnFixedUpdate = OnFixedUpdate;
            _tickMonoComponent.OnLateUpdate = OnLateUpdate;
        }
    
        public void Dispose()
        {
            Object.Destroy(_tickMonoComponent.gameObject);

            _onUpdateList.Clear();
            _onFixedUpdateList.Clear();
            _onLateUpdateList.Clear();
        }

        public void SubscribeOnUpdate(Action<float> action, float deltaTime = 0f, bool invokeOnSubscribe = false, bool overflowTick = false)
        {
            _onUpdateList.Add(new TickData
            {
                Id = ++_tickDataIdRef,
                Action = action,
                OverflowTick = overflowTick,
                DeltaTime = deltaTime,
                LastTickTime = Time.time,
                Subscriber = action.Target
            });

            if(invokeOnSubscribe)
            {
                action(0);
            }
        }

        public void SubscribeOnLateUpdate(Action<float> action, float deltaTime = 0f, bool invokeOnSubscribe = false, bool overflowTick = false)
        {
            _onLateUpdateList.Add(new TickData
            {
                Id = ++_tickDataIdRef,
                Action = action,
                OverflowTick = overflowTick,
                DeltaTime = deltaTime,
                LastTickTime = Time.time,
                Subscriber = action.Target
            });
            
            if(invokeOnSubscribe)
            {
                action(0);
            }
        }

        public void SubscribeOnFixedUpdate(Action<float> action, bool invokeOnSubscribe = false)
        {
            _onFixedUpdateList.Add(new TickData
            {
                Id = ++_tickDataIdRef,
                Action = action,
                Subscriber = action.Target
            });
            
            if(invokeOnSubscribe)
            {
                action(0);
            }
        }

        public void Unsubscribe(Action<float> action)
        {
            UnsubscribeOnUpdate(action);
            UnsubscribeOnFixedUpdate(action);
            UnsubscribeOnLateUpdate(action);
        }

        public void UnsubscribeOnUpdate(Action<float> action)
        {
            for(int i = 0; i < _onUpdateList.Count; i++)
            {
                if(_onUpdateList[i].Action == action && action.Target == _onUpdateList[i].Subscriber)
                {
                    _onUpdateList.RemoveAt(i);
                    return;
                }
            }
        }

        public void UnsubscribeOnFixedUpdate(Action<float> action)
        {
            for(int i = 0; i < _onFixedUpdateList.Count; i++)
            {
                if(_onFixedUpdateList[i].Action == action && action.Target == _onFixedUpdateList[i].Subscriber)
                {
                    _onFixedUpdateList.RemoveAt(i);
                    return;
                }
            }
        }

        public void UnsubscribeOnLateUpdate(Action<float> action)
        {
            for(int i = 0; i < _onLateUpdateList.Count; i++)
            {
                if(_onLateUpdateList[i].Action == action && action.Target == _onLateUpdateList[i].Subscriber)
                {
                    _onLateUpdateList.RemoveAt(i);
                    return;
                }
            }
        }

        public void UnsubscribeAllOnUpdate()
        {
            _onUpdateList.Clear();
        }

        public void UnsubscribeAllOnUpdate(object subscriber)
        {
            _onUpdateList.RemoveAll(data => data.Subscriber == subscriber);
        }

        public void UnsubscribeAllOnFixedUpdate()
        {
            _onFixedUpdateList.Clear();
        }

        public void UnsubscribeAllOnFixedUpdate(object subscriber)
        {
            _onFixedUpdateList.RemoveAll(data => data.Subscriber == subscriber);
        }

        public void UnsubscribeAllOnLateUpdate()
        {
            _onLateUpdateList.Clear();
        }

        public void UnsubscribeAllOnLateUpdate(object subscriber)
        {
            _onLateUpdateList.RemoveAll(data => data.Subscriber == subscriber);
        }

        public void UnsubscribeAll()
        {
            UnsubscribeAllOnUpdate();
            UnsubscribeAllOnFixedUpdate();
            UnsubscribeAllOnLateUpdate();
        }

        public void UnsubscribeAll(object subscriber)
        {
            UnsubscribeAllOnUpdate(subscriber);
            UnsubscribeAllOnFixedUpdate(subscriber);
            UnsubscribeAllOnLateUpdate(subscriber);
        }

        private void OnUpdate()
        {
            if(_onUpdateList.Count == 0)
            {
                return;
            }

            CheckAndUpdate(_onUpdateList);
        }

        private void OnFixedUpdate()
        {
            if(_onFixedUpdateList.Count == 0)
            {
                return;
            }

            var arrayCopy = _onFixedUpdateList.ToArray();

            for(int i = 0; i < arrayCopy.Length; i++)
            {
                arrayCopy[i].Action(Time.fixedTime);
            }
        }

        private void OnLateUpdate()
        {
            if(_onLateUpdateList.Count == 0)
            {
                return;
            }

            CheckAndUpdate(_onLateUpdateList);
        }

        private void CheckAndUpdate(List<TickData> list)
        {
            if(list.Count == 0)
            {
                return;
            }

            var arrayCopy = list.ToArray();

            for(int i = 0; i < arrayCopy.Length; i++)
            {
                var tickData = arrayCopy[i];
                var time = Time.time;

                if(time < tickData.LastTickTime + tickData.DeltaTime)
                {
                    continue;
                }

                var deltaTime = time - tickData.LastTickTime;
                var countBefore = list.Count;

                tickData.Action(deltaTime);

                // Check if the update was not unsubscribed during the call
                var index = i - (arrayCopy.Length - countBefore);
                if (list.Count > index && tickData == list[index])
                {
                    var overFlow = tickData.DeltaTime == 0 ? 0 : deltaTime % tickData.DeltaTime;

                    tickData.LastTickTime = tickData.OverflowTick ? time - overFlow : time;

                    list[index] = tickData;
                }
            }
        }

        /// <summary>
        /// Data wrapper for storing callback & related info for update actions
        /// </summary>
        private struct TickData
        {
            public int Id;
            public Action<float> Action;
            public bool OverflowTick;
            public float DeltaTime;
            public float LastTickTime;
            public object Subscriber;

            public bool Equals(TickData other)
            {
                return other.Id == Id;
            }

            public override bool Equals(object other)
            {
                return other is TickData && Equals((TickData)other);
            }

            public override int GetHashCode()
            {
                return Id;
            }

            public static bool operator ==(TickData a, TickData b)
            {
                return a.Id == b.Id;
            }

            public static bool operator !=(TickData a, TickData b)
            {
                return a.Id != b.Id;
            }
        }
    }
}