#region Author
/************************************************************************************************************
Author: EpixCode (Keven Poulin)
Website: http://www.EpixCode.com
GitHub: https://github.com/EpixCode
Twitter: https://twitter.com/EpixCode (@EpixCode)
LinkedIn: http://www.linkedin.com/in/kevenpoulin
************************************************************************************************************/
#endregion

#region Copyright
/************************************************************************************************************
Copyright (C) 2014 EpixCode

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files (the "Software"), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge, publish, distribute,
sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished 
to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
************************************************************************************************************/
#endregion

#region Class Documentation
/************************************************************************************************************
Class Name:     EventObject / ListenerObject
Namespace:      Com.EpixCode.Util.Events.EpixEvents
Type:           Util
Definition:
                An [EventObject] represent the relation of an [EventName] and a [Dispatcher]. It keeps the
                list of all [ListenerObject] registered to him to trigger their Callback when an event is
                raised.
************************************************************************************************************/
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#endregion

namespace Com.EpixCode.Util.Events.EpixEvents
{
    public class ListenerObject
    {
        private System.WeakReference _listener;
        public Action<EventObject> Callback { get; set; }
        public int Priority { get; set; }
        public object ObjectReference
        {
            get
            {
                return _listener.Target;
            }
        }

        public bool IsAlive
        {
            get
            {
                return (bool)(this._listener != null && this._listener.IsAlive && !this._listener.Target.Equals(null));
            }
        }

        public ListenerObject(object listener, Action<EventObject> callback, int priority = 1)
        {
            this._listener = new System.WeakReference(listener);
            this.Callback = callback;
            this.Priority = priority;
        }
    }

    public class EventObject
    {
        //Reference Holder
        private System.WeakReference _dispatcher;
        private string _event;
        private object[] _params;
        private Action<EventObject> _onDestroyCallback;
        private List<ListenerObject> _listenerList;

        public object Dispatcher
        {
            get
            {
                return _dispatcher.Target;
            }
        }

        public bool IsAlive
        {
            get
            {
                return (bool)(this._dispatcher != null && this._dispatcher.IsAlive && !this._dispatcher.Target.Equals(null));
            }
        }

        public string EventName
        {
            get
            {
                return _event;
            }
        }

        public object[] Params
        {
            get
            {
                return _params;
            }
        }

        //Management
        internal EventObject(string aEvent, object aDispatcher)
        {
            this._event = aEvent;
            this._dispatcher = new System.WeakReference(aDispatcher);

            Init();
        }

        internal void Init()
        {
            _listenerList = new List<ListenerObject>();
        }

        internal void RegisterDestroyCallback(Action<EventObject> aDestroyCallback)
        {
            _onDestroyCallback = aDestroyCallback;
        }

        internal void Destroy()
        {
            if (_onDestroyCallback != null)
            {
                _onDestroyCallback(this);
                _onDestroyCallback = null;
            }

            _event = "";
            _params = null;
            _dispatcher = null;
            _listenerList.Clear();
        }

        internal void RegisterEventListener(object aListener, Action<EventObject> aMethod, int aPriority = 1)
        {
            ListenerObject listener = new ListenerObject(aListener, aMethod, aPriority);
            _listenerList.Add(listener);
            _listenerList = _listenerList.OrderBy(o => o.Priority).ToList<ListenerObject>();

        }

        internal void UnregisterEventListenerObject(ListenerObject aListener)
        {
            if (aListener != null && _listenerList.Contains(aListener))
            {
                _listenerList.Remove(aListener);
            }
        }

        internal void UnregisterEventListener(object aListener, Action<EventObject> aMethod = null)
        {
            int index = 0;
            bool eventRemoved = false;

            while (!eventRemoved)
            {
                if (index >= _listenerList.Count)
                {
                    break;
                }

                ListenerObject listener = _listenerList[index];
                if ((listener.ObjectReference.Equals(aListener) && aMethod != null && listener.Callback.Equals(aMethod)) ||
                    (listener.ObjectReference.Equals(aListener) && aMethod == null))
                {
                    UnregisterEventListenerObject(listener);
                    eventRemoved = true;
                }
                else
                {
                    if (listener.IsAlive)
                    {
                        index++;
                    }
                    else
                    {
                        UnregisterEventListenerObject(listener);
                    }
                }
            }

            if (!eventRemoved)
            {
                string prefix = "";
                if (aMethod != null)
                {
                    prefix += "The method [" + aMethod.ToString() + "] from ";
                }
                Debug.LogWarning("[EventObject.cs] - UnregisterEventListener() - " + prefix + aListener.ToString() + "] doesn't seem to be registered for the event: " + _event + ".");
            }

            if (_listenerList.Count <= 0)
            {
                Destroy();
            }
        }

        internal void Dispatch(object[] aParams)
        {
            _params = aParams;
            Dispatch();
			if(_params != null)
			{
            	Array.Clear(_params, 0, _params.Length);
			}
        }

        internal void Dispatch()
        {
            if (!this.IsAlive)
            {
                Destroy();
                return;
            }

            int index = 0;
            while (index < _listenerList.Count)
            {
                ListenerObject listener = _listenerList[index];

                if (listener.IsAlive)
                {
                    listener.Callback(this);
                    index++;
                }
                else
                {
                    UnregisterEventListenerObject(listener);
                }
            }

            if (_listenerList.Count <= 0)
            {
                Destroy();
            }
        }

        public object GetParam(int aIndex)
        {
            if (aIndex < _params.Length)
            {
                return _params[aIndex];
            }
            else
            {
                Debug.LogWarning("[EventObject.cs] - GetParam() - Can't retrieve param at index [" + aIndex + "] in the param list [Length = " + _params.Length + "]");
                return null;
            }
        }

        public override string ToString()
        {
            return ("[" + _dispatcher + "] - " + _event);
        }
    }
}
