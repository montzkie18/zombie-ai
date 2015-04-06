using UnityEngine;
using System.Collections;

namespace Day1.ZombieStates {
	public class IdleAction : ZombieAction {

		float timeLastIdle = 0;
		bool isOnIdle = false;

		public override void Init() {
			base.Init();
			CheckIfShouldWait();
			audio3.mute = true;
		}

		public override void Continue() {
			base.Continue();
			CheckIfShouldWait();
		}

		public override void Hold() {
			base.Hold();
			SaveLastWaitTime();
		}

		public override void End() {
			base.End();
		}

		void CheckIfShouldWait() {
			if((Time.time - timeLastIdle) > GameConfig.ZOMBIE_MIN_DELAY_BETWEEN_IDLE) {
				isOnIdle = true;
				zombieAnimation.animateIdle();
				FinishState(GameConfig.ZOMBIE_MAX_IDLE_TIME);
			}else{
				FinishState();
			}
		}

		void SaveLastWaitTime() {
			CancelInvoke("FinishState");
			if(isOnIdle) {
				isOnIdle = false;
				timeLastIdle = Time.time;
			}
		}
	}
}

