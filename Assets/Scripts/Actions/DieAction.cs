using UnityEngine;
using System.Collections;

namespace Day1.ZombieStates {
	public class DieAction : ZombieAction {

		public override void Init() {
			base.Init();
			animator.enabled = false;
			for(int i=0; i<audioSources.Length; ++i)
				audioSources[i].Stop();
		}

	}
}	