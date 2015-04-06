using UnityEngine;
using System.Collections;

namespace Day1.ZombieStates {
	public class ZombieAction : MonoBehaviour {

		public const string EVENT_FINISHED = "ZombieState.EVENT_FINISHED";
		
		private Animator _animator;
		protected Animator animator {
			get {
				if(_animator == null)
					_animator = gameObject.GetComponentInChildren<Animator>();
				return _animator;
			}
		}

		private IEnemyAnimation _zombieAnimation;
		protected IEnemyAnimation zombieAnimation {
			get {
				if(_zombieAnimation == null)
					_zombieAnimation = (IEnemyAnimation)GetComponent(typeof(IEnemyAnimation));
				return _zombieAnimation;
			}
		}

		private NavMeshAgent _navMeshAgent;
		protected NavMeshAgent navMeshAgent {
			get {
				if(_navMeshAgent == null)
					_navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
				return _navMeshAgent;
			}
		}

		private AudioSource[] _audioSources;
		protected AudioSource[] audioSources {
			get {
				if(_audioSources == null)
					_audioSources = gameObject.GetComponents<AudioSource>();
				return _audioSources;
			}
		}

		protected AudioSource audio1 { get { return audioSources[0]; } }
		protected AudioSource audio2 { get { return audioSources[1]; } }
		protected AudioSource audio3 { get { return audioSources[2]; } }
		protected AudioSource audio4 { get { return audioSources[3]; } }

		public int type {get; set;}

		public Transform player {get; set;}

		public virtual void Init() {
			enabled = true;
		}

		public virtual void Hold() {
			enabled = false;
		}

		public virtual void Continue() {
			enabled = true;
		}

		public virtual void End() {
			enabled = false;
		}

		protected void FinishState() {
			this.DispatchEvent(EVENT_FINISHED, new object[1]{this});
		}

		protected void FinishState(float interval) {
			Invoke("FinishState", interval);
		}

		void OnDestroy() {
			CancelInvoke();
		}
	}
}

