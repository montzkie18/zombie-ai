using UnityEngine;
using System.Collections;

namespace Day1.ZombieStates {
	public class WanderingAction : MovingAction {

		Vector3 targetLocation = Vector3.zero;

		int visibleLayer = 0;
		int visibleLayerMask = 0;
		int navMeshLayerMask = 0;

		public override void Init() {
			base.Init();
			visibleLayer = LayerMask.NameToLayer("Sight");
			visibleLayerMask = 0x01 << visibleLayer;
			navMeshLayerMask = 1 << NavMesh.GetAreaFromName("Default");

			targetLocation = GetDestination();
			navMeshAgent.SetDestination(targetLocation);
			navMeshAgent.speed = GameConfig.WALKING_SPEED;
			navMeshAgent.acceleration = GameConfig.WALKING_SPEED;

			audio3.mute = false;
		}

		public override void Hold() {
			base.Hold();
			navMeshAgent.Stop();
		}

		public override void Continue() {
			base.Continue();
			navMeshAgent.Resume();
		}

		public override void End() {
			base.End();
			navMeshAgent.Stop();
		}

		Vector3 GetDestination() {
			var walkRadius = navMeshAgent.speed;
			if(Application.loadedLevelName == "Game")
			   walkRadius *= GameConfig.ZOMBIE_MIN_DELAY_BETWEEN_IDLE;

			Vector3 destination = transform.forward * (walkRadius / 2f) + Random.insideUnitSphere * walkRadius;
			destination += transform.position;
			
			RaycastHit rHit;
			if(Physics.Linecast(transform.position, destination, out rHit, visibleLayerMask)) {
				if(rHit.transform.gameObject.layer == visibleLayer) {
					destination = rHit.point;
				}
			}
			
			NavMeshHit nHit;
			if(NavMesh.SamplePosition(destination, out nHit, walkRadius, navMeshLayerMask)) {
				destination = nHit.position;
			}

			return destination;
		}
		
		void Update() {
			UpdateMovement();
			float distance = navMeshAgent.remainingDistance;
			if(navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
			   distance != Mathf.Infinity && MathUtility.NearlyEqual(distance, 0f, 0.01f)) {
				FinishState();
			}
		}
	}
}

