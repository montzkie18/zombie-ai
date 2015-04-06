using UnityEngine;
using System.Collections;

public interface IEnemyAnimation {

	void animateIdle();
	
	void animateWalk(float speed);
	
	void animateRun(float speed);
	
	void animateEat(bool fade);
	
	void animateEatDone();
	
	void animateDeath(bool fade);

	void animateAttention();
	
	void animateScream(bool fade);
	
	void animateScreamDone();
	
	void animateAttack(int power);

	void animateDamage(float power);

	void setAnimationSpeed(float speed);

}
