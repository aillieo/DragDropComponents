using System.Collections.Generic;
using UnityEngine;

#if USE_LUA_FRAMEWORK
using LuaInterface;
#endif

using System;

namespace AillieoUtils
{

    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public abstract class DragDropPair : MonoBehaviour
    {
        protected Dictionary<DragDropEventTriggerType, Action<DragDropEventData>> csCallbacks = new Dictionary<DragDropEventTriggerType, Action<DragDropEventData>>();

#if USE_LUA_FRAMEWORK
        protected Dictionary<DragDropEventTriggerType, LuaFunction> luaCallbacks = new Dictionary<DragDropEventTriggerType, LuaFunction>();
#endif

        [SerializeField]
        [Tooltip("无视matchingChannel 可以与任何的matchingChannel匹配")]
        public bool universalMatching;


        [SerializeField]
        [Tooltip("用于筛选可匹配的item和target")]
        public int matchingChannel;


        [SerializeField]
        [Tooltip("匹配双方的标记")]
        public int matchingTag;


        protected bool HandleEventForType(DragDropEventTriggerType type, DragDropEventData eventData)
        {

            //Debug.LogError(string.Format("name={0} event={1}",name,type));

            bool handleEvent = false;
            if (csCallbacks.ContainsKey(type))
            {
                csCallbacks[type](eventData);
                handleEvent = true;
            }
#if USE_LUA_FRAMEWORK
            if (luaCallbacks.ContainsKey(type))
            {
                luaCallbacks[type].Call(eventData);
                handleEvent = true;
            }
#endif
            return handleEvent;
        }

        public void AddCallback(DragDropEventTriggerType type, Action<DragDropEventData> function)
        {
            if (csCallbacks.ContainsKey(type))
            {
                csCallbacks[type] += function;
            }
            csCallbacks[type] = function;
        }

        public void RemoveCallback(DragDropEventTriggerType type, Action<DragDropEventData> function)
        {
            if (csCallbacks.ContainsKey(type))
            {
                csCallbacks[type] -= function;
                if(csCallbacks[type].GetInvocationList().Length == 0)
                {
                    csCallbacks.Remove(type);
                }
            }
        }

#if USE_LUA_FRAMEWORK

        public void SetLuaCallback(DragDropEventTriggerType type, LuaFunction function)
        {
            if (luaCallbacks.ContainsKey(type))
            {
                luaCallbacks[type].Dispose();
                if(null == function)
                {
                    luaCallbacks.Remove(type);
                }
            }
            if (null != function)
            {   
                luaCallbacks[type] = function;
            }
        }

#endif

        public void OnDestroy()
        {
#if USE_LUA_FRAMEWORK
            foreach (var lf in luaCallbacks)
            {
                lf.Value.Dispose();
            }
            luaCallbacks.Clear();

#endif

            csCallbacks.Clear();
        }

    }
}
