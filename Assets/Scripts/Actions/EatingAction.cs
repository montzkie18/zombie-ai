using UnityEngine;
using System.Collections;
using System;

namespace Day1.ZombieStates {
	public class EatingAction : ZombieAction {

		private GameObject playerObject;
		private GameObject playerSpine;

		public override void Init() {
			base.Init();
			StartEating();
		}

		public override void End() {
			base.End();
			zombieAnimation.animateEatDone();
		}

		private void StartEating() {
			zombieAnimation.setAnimationSpeed(1.0f);
			zombieAnimation.animateEat(false);

			audio3.mute = true;
			audio1.volume = 1f;
			audio1.clip = SoundEffects.CAPTURED_AUDIO;
			audio1.loop = true;
			audio1.PlayDelayed(1);

			playerObject = player.gameObject;
			playerSpine = GameObject.Find("Alpha:Spine1");
			StartCoroutine(BloodEffectCoroutine(() => {
				audio1.volume = 0.6f;
				audio1.loop = false;
				audio3.mute = false;
				FinishState();
			}));
		}
		
		private void DoBloodEffect() {
			Vector3 pos = (playerSpine.transform.position + transform.position) / 2;
			pos.y = transform.position.y + 2.1f;
			Instantiate(VisualEffects.BLOOD_EFFECTS, pos, Quaternion.Euler(0, 0, 0));
		}
		
		/// wander after blood effect
		private IEnumerator BloodEffectCoroutine(Action afterBloodEffects) {
			int bloodEffectPlayCount = Mathf.RoundToInt(GameConfig.DEAD_TIME / GameConfig.BLOOD_EFFECT_TIME) + 1;
			for (int i = 0; i < bloodEffectPlayCount; i++) {
				yield return new WaitForSeconds(GameConfig.BLOOD_EFFECT_TIME);
				DoBloodEffect();
			}
			yield return new WaitForSeconds(GameConfig.BLOOD_EFFECT_TIME);
			afterBloodEffects();
		}

	}
}

