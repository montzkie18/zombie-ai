using UnityEngine;
using System.Collections;

namespace Day1.ZombieStates {
	public class HitAction : ZombieAction {

		public override void Init() {
			base.Init();

			//!TODO: Figure out the damage dealth to zombie
			float power = Random.Range(0f, 1f);
			zombieAnimation.animateDamage(0);

			float animTime = 0.875f;
			if(power >= 1f) {
				animTime = 1.625f;
			}else if(power >= 0.5f) {
				animTime = 1.25f;
			}else{
				animTime = 0.875f;
			}

			Invoke("FinishState", animTime);
		}
	}
}

