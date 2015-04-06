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
Class Name:     EventExtension
Namespace:      Com.EpixCode.Util.Events.EpixEvents
Type:           Native Extension
Definition:
                [EventExtension] use C# Native Extension to inject event system functionalities
                inside any objects.
************************************************************************************************************/
#endregion

#region Using
using Com.EpixCode.Util.Events.EpixEvents;
using System;
#endregion

public static class EventExtension
{
    //Local Event
    public static void AddEventListener(this object aObject, string aEvent, object aDispatcher, Action<EventObject> aMethod, int aPriority = 1)
    {
        EventDispatcher.AddEventListener(aEvent, aDispatcher, aObject, aMethod, aPriority);
    }

    public static void RemoveEventListener(this object aObject, string aEvent, object aDispatcher, Action<EventObject> aMethod)
    {
        EventDispatcher.RemoveEventListener(aEvent, aDispatcher, aObject, aMethod);
    }

    public static void RemoveAllEventListener(this object aObject)
    {
        EventDispatcher.RemoveAllEventListner(aObject);
    }

    public static void DispatchEvent(this object aObject, string aEvent)
    {
        EventDispatcher.DispatchEvent(aEvent, aObject);
    }

    public static void DispatchEvent(this object aObject, string aEvent, object[] aParams)
    {
        EventDispatcher.DispatchEvent(aEvent, aObject, aParams);
    }

    //Global Event
    public static void AddGlobalEventListener(this object aObject, string aEvent, Action<EventObject> aMethod, int aPriority = 1)
    {
        EventDispatcher.AddEventListener(aEvent, GlobalDispatcher.Instance, aObject, aMethod, aPriority);
    }

    public static void RemoveGlobalEventListener(this object aObject, string aEvent, Action<EventObject> aMethod)
    {
        EventDispatcher.RemoveEventListener(aEvent, GlobalDispatcher.Instance, aObject, aMethod);
    }

    public static void DispatchGlobalEvent(this object aObject, string aEvent)
    {
        EventDispatcher.DispatchEvent(aEvent, GlobalDispatcher.Instance);
    }

    public static void DispatchGlobalEvent(this object aObject, string aEvent, object[] aParams)
    {
        EventDispatcher.DispatchEvent(aEvent, GlobalDispatcher.Instance, aParams);
    }
}
