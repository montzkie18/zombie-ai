using UnityEngine;
using System.Collections;

namespace Day1.ZombieStates {
	public class ScreamAction : ZombieAction {

		public override void Init() {
			base.Init();
			audio4.PlayOneShot(SoundEffects.SCREAM_AUDIO);
			zombieAnimation.animateScream(false);
			FinishState(3.5f);
		}

		public override void Hold() {
			base.Hold();
			CancelInvoke();
		}

		public override void Continue() {
			base.Continue();
			FinishState();
		}
		
		public override void End() {
			base.End();
			zombieAnimation.animateScreamDone();
			CancelInvoke();
		}

	}
}

