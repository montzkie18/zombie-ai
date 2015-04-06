using UnityEngine;
using System.Collections;

namespace Day1.ZombieStates {
	public class MovingAction : ZombieAction {

		private float runSpeed = 6.0f;
		private float walkSpeed = 2.0f;
		private float animationSpeed = 1.0f;

		public override void Hold() {
			base.Hold();
			zombieAnimation.setAnimationSpeed(1.0f);
		}

		public override void End() {
			base.End();
			zombieAnimation.setAnimationSpeed(1.0f);
		}

		protected void UpdateMovement() {
			animationSpeed = 1.0f;
			float velocity = navMeshAgent.velocity.magnitude;
			if(velocity > runSpeed * 0.8f) {
				animationSpeed = Mathf.Max(velocity/runSpeed, 1.0f);
				zombieAnimation.animateRun(velocity);
			}
			else if(velocity > 0f) {
				animationSpeed = velocity / walkSpeed;
				zombieAnimation.animateWalk(velocity);
			}
			else {
				zombieAnimation.animateIdle();
			}
			zombieAnimation.setAnimationSpeed(animationSpeed);
		}

	}
}

