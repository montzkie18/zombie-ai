WeakDictionary for Unity (Version 2.0)
==============

Why creating a WeakDictionary implementation?
--------------
**WeakDictionary** is quite common implementation. It is useful to avoid memory leaks when you build third party system that use resource references but should never owned them. That's why [*ConditionalWeakTable*](http://msdn.microsoft.com/en-us/library/dd287757(v=vs.110).aspx) has been added in .NET Framework 4. Unfortunately, Unity cannot use *ConditionalWeakTable* due to the Mono version it use. **WeakDictionary** has been created to fix this problem.

How should I use it?
--------------
**WeakDictionary** is dead simple to use. It's based on the exact same syntax as the basic [*Dictionary*](http://msdn.microsoft.com/en-us/library/xfhwa508(v=vs.110).aspx) coming from the *System.Collections.Generic* namespace of .NET. You simply have to import the content of this GitHub repository in your project.

	using Com.EpixCode.Util.WeakReference.WeakDictionary

	WeakDictionary<object, string> myWeakDictionary = new WeakDictionary<object, string>();
    myWeakDictionary.Add(myObject, "myString");

What are the supported platform?
--------------
**WeakDictionary** is mostly build for Unity. Because it is hardly based on the [*WeakReference*](http://msdn.microsoft.com/en-us/library/system.weakreference(v=vs.110).aspx) class, **WeakDictionary** could be use in with .NET Framework 1.1 and higher.

What changed?
--------------
**Version 2.0** *- 04/05/2014*
* New Implementation of WeakDictionary.
* 100% WeakRefrence (No more hard reference on HashCode collision).
* All class needed are now internal to weakDictionary.

**Version 1.0** *- 22/04/2014* 
* Basic WeakDictionary implementation.
* Facultative AutoClean by setting "IsAutoClean" (True by default).
* Hard reference created when HashCode collision detected.

How could I get more information?
--------------
Get more info at [www.EpixCode.com](http://epixcode.com/)