EpixEvents - Event-System for Unity
===================

EpixEvents is a Simple, Reliable and Blazing-Fast library to create an Event-Driven architecture in C#/Unity. It is heavily inspired by the powerful Event-System from ActionScript 3 flavored with additional built-in features such as the ability to dispatch global events. It is also based on the WeakDictionary implementation from EpixCode to reduce the chance of creating memory leaks down to zero. Using Native Extensions from C#, EpixEvents require no efforts to setup: Once the code is in your project, absolutely any object can now listen or dispatch events!


How to use Local Event?
--------------

**Listen Local Event**
```c#
this.AddEventListener("SwitchOn", gameObject, OnSwitchIsOn);

//...

private void OnSwitchIsOn(EventObject aEvent)
{
    //...
}
```

**Dispatch Local Event**
```c#
gameObject.DispatchEvent("SwitchOn");
```


How to use Global Event?
--------------

**Listen Global Event**
```c#
this.AddGlobalEventListener("GameOver", OnGameOver);

//...

private void OnGameOver(EventObject aEvent)
{
    //...
}
```

**Dispatch Global Event**
```c#
this.DispatchGlobalEvent("GameOver");
```


Why using it?
--------------

**Simplicity** - *“Simple in his conception, simple to use!”*

EpixEvents use the Native Extension feature of C# to inject base event functionality directly into native object class. That means that from the get-go any of your script is now ready to listen and/or dispatch events. Just like ActionScript, “event” are as simple as a string key and have the ability to send parameters along the way. No need of specific prefab on the scene or any configurations.

**Reliability** - *“Don’t be afraid to overuse it!”*

Most of custom Event-System available are prone to memory leak because they expects the developers to remove their "Listener" when they are not needed anymore. Even if this is a very good practice, it’s utopic to think that none of them will be forgotten, preventing objects to be correctly garbage collected. For that reason, EpixEvents is based on weak references. That means that no object will remain in memory because it’s still attached to an event. The next time this event will be dispatched, EpixEvents will detect that an object is missing and will proceed to an auto-cleaning.

**Speed** - *“Because nobody has the time to wait!”*

Another problem with most of custom Event-System is their speed. Often based on reflectivity, the process slow down dramatically when the traffic gets intense. Others has remedy to that problem by using Time-Slicing to reduce the amount of dispatch on a single frame. EpixEvents is based on Delegate (Function pointer) which allow an incredibly fast execution time and have the advantage of being synchronous. In other words, you are sure that your event will get spread in your code as fast as it can possibly be.


What changed?
--------------
**Version 1.0** *- 11/05/2014* 
* Basic implementation.
* Local / Global dispatch
* 100% WeakReference Based (No chance to create memory leak)
* Ability to sort listener priority.
* Native Extension to object (everything can listen/dispatch events)

How could I get more information?
--------------
Get more info at [www.EpixCode.com](http://epixcode.com/)