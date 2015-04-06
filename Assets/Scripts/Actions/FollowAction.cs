using UnityEngine;
using System.Collections;

namespace Day1.ZombieStates {
	public class FollowAction : MovingAction {

		Vector3 targetPosition = Vector3.zero;
		float timeSinceLastGrowl = 0f;
		
		public override void Init() {
			base.Init();

			navMeshAgent.speed = GameConfig.RUNNING_SPEED;
			navMeshAgent.acceleration = GameConfig.RUNNING_SPEED;

			audio3.mute = false;

			timeSinceLastGrowl = 0f;
		}

		public override void Hold() {
			base.Hold();
			navMeshAgent.Stop();
		}

		public override void Continue() {
			base.Continue();
			navMeshAgent.SetDestination(targetPosition);
		}
		
		public override void End() {
			base.End();
			navMeshAgent.Stop();
		}
		
		void Update() {
			if(player == null) {
				FinishState();
			}else{
				UpdateDirection();
				UpdateMovement();
				UpdateSound();
			}
		}

		void UpdateDirection() {
			var targetVector = transform.position - player.position;
			targetVector = targetVector.normalized;
			targetPosition = player.position + targetVector;
			navMeshAgent.SetDestination(targetPosition);
		}

		void UpdateSound() {
			timeSinceLastGrowl += Time.deltaTime;
			if (timeSinceLastGrowl > GameConfig.GROWLING_INTERVAL) {
				timeSinceLastGrowl = 0;
				audio1.clip = SoundEffects.GROWL_AUDIO;
				audio1.Play();
			}
		}

	}
}

