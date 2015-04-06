using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventObject = Com.EpixCode.Util.Events.EpixEvents.EventObject;
using Day1.ZombieStates;

public enum EnemyActionType {
	Idle,
	Wandering,
	
	Screaming,
	Following,
	Attacking,
	
	Eating,
	Hit,
	Dead
}

/// <summary>
/// Zombie state machine
/// </summary>
public class EnemyStateManager : MonoBehaviour {
	
	Dictionary<EnemyActionType, ZombieAction> actionMap = new Dictionary<EnemyActionType, ZombieAction>();
	Stack<ZombieAction> actionStack = new Stack<ZombieAction>();

	private EnemyActionType _currentAction = EnemyActionType.Idle;
	public EnemyActionType currentAction { get { return _currentAction; } }

	private Transform player = null;

	// Use this for initialization
	void Start () {
		InitSensors();
		RegisterAction(EnemyActionType.Idle, CreateComponent<IdleAction>(gameObject));
		RegisterAction(EnemyActionType.Wandering, CreateComponent<WanderingAction>(gameObject));
		RegisterAction(EnemyActionType.Screaming, CreateComponent<ScreamAction>(gameObject));
		RegisterAction(EnemyActionType.Following, CreateComponent<FollowAction>(gameObject));
		RegisterAction(EnemyActionType.Attacking, CreateComponent<AttackAction>(gameObject));
		RegisterAction(EnemyActionType.Eating, CreateComponent<EatingAction>(gameObject));
		RegisterAction(EnemyActionType.Hit, CreateComponent<HitAction>(gameObject));
		RegisterAction(EnemyActionType.Dead, CreateComponent<DieAction>(gameObject));
		ChangeAction(EnemyActionType.Idle);
	}

	void OnDestroy() {
		foreach(EnemyActionType type in actionMap.Keys) {
			this.RemoveEventListener(ZombieAction.EVENT_FINISHED, actionMap[type], HandleStateFinished);
		}
		CleanSensors();
	}

	T CreateComponent<T>(GameObject go) where T : Component {
		T component = go.GetComponent<T>();
		if(component == null)
			component = go.AddComponent<T>();
		return component;
	}

	void InitSensors() {
		this.AddEventListener(EnemySensors.EVENT_PLAYER_VISIBLE, gameObject, HandleSensorDetected);
		this.AddEventListener(EnemySensors.EVENT_PLAYER_HIDDEN, gameObject, HandleSensorDetected);
		this.AddEventListener(EnemySensors.EVENT_PLAYER_WITHIN_REACH, gameObject, HandleSensorDetected);
		this.AddEventListener(EnemySensors.EVENT_PLAYER_OUT_OF_REACH, gameObject, HandleSensorDetected);
		this.AddEventListener(EnemySensors.EVENT_ZOMBIE_DAMAGED, gameObject, HandleSensorDetected);
		this.AddEventListener(EnemySensors.EVENT_ZOMBIE_DIED, gameObject, HandleSensorDetected);
	}

	void CleanSensors() {
		this.RemoveEventListener(EnemySensors.EVENT_PLAYER_VISIBLE, gameObject, HandleSensorDetected);
		this.RemoveEventListener(EnemySensors.EVENT_PLAYER_HIDDEN, gameObject, HandleSensorDetected);
		this.RemoveEventListener(EnemySensors.EVENT_PLAYER_WITHIN_REACH, gameObject, HandleSensorDetected);
		this.RemoveEventListener(EnemySensors.EVENT_PLAYER_OUT_OF_REACH, gameObject, HandleSensorDetected);
		this.RemoveEventListener(EnemySensors.EVENT_ZOMBIE_DAMAGED, gameObject, HandleSensorDetected);
		this.RemoveEventListener(EnemySensors.EVENT_ZOMBIE_DIED, gameObject, HandleSensorDetected);
	}

	void RegisterAction(EnemyActionType type, ZombieAction action) {
		if(!actionMap.ContainsKey(type))
			actionMap.Add(type, action);
		this.AddEventListener(ZombieAction.EVENT_FINISHED, action, HandleStateFinished);
		action.enabled = false;
		action.type = (int)type;
	}

	void ChangeAction(EnemyActionType type) {
		if(actionStack.Count > 0 && 
		   actionStack.Peek() == actionMap[type])
			// ignore if already in this state
			return;

		while(actionStack.Count > 0) {
			actionStack.Peek().End();
			actionStack.Pop();
		}

		_currentAction = type;
		actionStack.Push(actionMap[_currentAction]);
		actionStack.Peek().player = player;
		actionStack.Peek().Init();
	}

	void PushAction(EnemyActionType type) {
		if(actionStack.Count > 0 && 
		   actionStack.Peek() == actionMap[type]) {
			// restart the current state instead of pushing a new one
			// e.g. allows hit animation to just restart
			actionStack.Peek().player = player;
			actionStack.Peek().Init();
			return;
		}

		if(actionStack.Count > 0)
			actionStack.Peek().Hold();

		_currentAction = type;
		actionStack.Push(actionMap[_currentAction]);
		actionStack.Peek().player = player;
		actionStack.Peek().Init();
	}

	void PopAction() {
		if(actionStack.Count > 0)
			actionStack.Pop().End();

		if(actionStack.Count > 0) {
			_currentAction = (EnemyActionType)actionStack.Peek().type;
			actionStack.Peek().Continue();
		} else {
			PushAction(EnemyActionType.Idle);
		}
	}

	/// <summary>
	/// Explicitly define default state transitions here
	/// </summary>
	/// <param name="eventObject">Event object.</param>
	void HandleStateFinished(EventObject eventObject) {
		if(IsDead()) return;

		switch(_currentAction) {
		case EnemyActionType.Idle:
			// After idle time is over, start walking in random places
			PushAction(EnemyActionType.Wandering);
			break;

		case EnemyActionType.Wandering:
			// Return to previous state which should always be idle
			PopAction();
			break;

		case EnemyActionType.Hit:
			// After zombie got shot, automatically trigger attention
			// and start chasing the player who made the shot
			PopAction();
			if(_currentAction != EnemyActionType.Screaming &&
			   _currentAction != EnemyActionType.Following &&
			   _currentAction != EnemyActionType.Attacking)
				ChangeAction(EnemyActionType.Screaming);
			break;

		case EnemyActionType.Screaming:
			// After the attention animation, start chasing player
			ChangeAction(EnemyActionType.Following);
			break;

		case EnemyActionType.Attacking:
			// Go back to previous state which is always FollowState
			PopAction();
			break;

		default:
			// default back to idle state
			ChangeAction(EnemyActionType.Idle);
			break;
		}
	}

	/// <summary>
	/// Explicitly define state transitions based on external signals
	/// like collisions, sound, light, etc...
	/// </summary>
	/// <param name="eventObject">Event object.</param>
	void HandleSensorDetected(EventObject eventObject) {
		if(IsDead()) return;

		switch(eventObject.EventName) {
		// when player is within line of sight of player
		case EnemySensors.EVENT_PLAYER_VISIBLE:
			GameObject playerObject = (GameObject)eventObject.GetParam(0);
			player = playerObject.transform.root;
			if(_currentAction != EnemyActionType.Screaming &&
			   _currentAction != EnemyActionType.Following &&
			   _currentAction != EnemyActionType.Attacking)
				ChangeAction(EnemyActionType.Screaming);
			break;

		case EnemySensors.EVENT_PLAYER_HIDDEN:
			player = null;
			if(_currentAction == EnemyActionType.Screaming ||
			   _currentAction == EnemyActionType.Following)
				ChangeAction(EnemyActionType.Idle);
			break;

		// when player is within reach of zombie
		// - either try to kill the player
		// - or start eating *nomnomnom*
		case EnemySensors.EVENT_PLAYER_WITHIN_REACH:
			PlayerStatus playerStatus = (PlayerStatus)eventObject.GetParam(0);
			player = playerStatus.transform.root;
			if(playerStatus.HitPoint > 0f) {
				if(_currentAction != EnemyActionType.Following)
					ChangeAction(EnemyActionType.Following);
				if(_currentAction != EnemyActionType.Attacking)
					PushAction(EnemyActionType.Attacking);
			}else{
				if(_currentAction != EnemyActionType.Eating)
					ChangeAction(EnemyActionType.Eating);
			}
			break;

		case EnemySensors.EVENT_PLAYER_OUT_OF_REACH:
			PopAction();
			break;

		case EnemySensors.EVENT_ZOMBIE_DAMAGED:
			PushAction(EnemyActionType.Hit);
			break;

		case EnemySensors.EVENT_ZOMBIE_DIED:
			ChangeAction(EnemyActionType.Dead);
			break;

		default:
			Debug.Log("Unknown sensor event: " + eventObject.EventName);
			break;
		}
	}

	public bool IsDead() {
		return _currentAction == EnemyActionType.Dead;
	}

	public bool IsEating() {
		return _currentAction == EnemyActionType.Eating;
	}

	public bool IsAttacking() {
		return _currentAction == EnemyActionType.Attacking;
	}

	public bool IsChasing() {
		return _currentAction == EnemyActionType.Screaming ||
			_currentAction == EnemyActionType.Following ||
				_currentAction == EnemyActionType.Attacking;
	}
}
