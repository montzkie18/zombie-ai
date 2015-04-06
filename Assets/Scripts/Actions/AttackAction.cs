using UnityEngine;
using System.Collections;

namespace Day1.ZombieStates {
	public class AttackAction : ZombieAction {

		public override void Init() {
			base.Init();
			Attack();
		}

		public override void Hold() {
			base.Hold();
			CancelAttacks();
		}

		public override void Continue() {
			base.Continue();
			Attack();
		}

		public override void End() {
			base.End();
			CancelAttacks();
		}

		void Attack() {
			audio1.PlayOneShot(SoundEffects.ATTACK_AUDIO);
			zombieAnimation.animateAttack(0);
			WaitForAnotherAttack();
		}

		void WaitForAnotherAttack() {
			Invoke("Attack", GameConfig.ATTACKING_INTERVAL);
		}

		void CancelAttacks() {
			CancelInvoke();
		}

	}
}