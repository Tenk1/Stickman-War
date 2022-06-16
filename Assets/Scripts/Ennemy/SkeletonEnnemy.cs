using UnityEngine;
using System.Collections;
using RootMotion.Dynamics;
using SDD.Events;

public class SkeletonEnnemy : MonoBehaviour
{

	public Animator animator;
	public PuppetMaster puppetMaster;

	public ConfigurableJoint[] leftLeg;
	public ConfigurableJoint[] rightLeg;
	public ConfigurableJoint[] deadly;
	EnemyLife enemyLife;

	bool leftLegRemoved, rightLegRemoved, headRemoved;

	void Start()
	{
		// Register to get a call from PM when a muscle is removed or disconnected
		puppetMaster.OnMuscleRemoved += OnMuscleDisconnected;
		puppetMaster.OnMuscleDisconnected += OnMuscleDisconnected;
	}

	public void OnRebuild()
	{
		//puppetMaster.state = PuppetMaster.State.Alive;
		animator.SetFloat("Legs", 2);
		animator.Play("Move", 0, 0f);
		leftLegRemoved = false;
		rightLegRemoved = false;
	}

	// Called by PM when a muscle is removed (once for each removed muscle)
	void OnMuscleDisconnected(Muscle m)
	{
		bool isLeft = false;

		// If one of the legs is missing, play the "jump on one leg" animation. If both, set PM state to Dead.
		if (IsLegMuscle(m, out isLeft))
		{
			if (isLeft) leftLegRemoved = true;
			else rightLegRemoved = true;

			if (leftLegRemoved && rightLegRemoved)
			{
				EventManager.Instance.Raise(new EnemyLifeEvent() { eLife = GetComponent<EnemyLife>().m_maxLife, obj = transform.root.gameObject });
			}
			else
			{
				animator.SetFloat("Legs", 1);
			}
		}

		if (IsDeadlyMuscle(m))
		{
			EventManager.Instance.Raise(new EnemyLifeEvent() { eLife = GetComponent<EnemyLife>().m_maxLife, obj = transform.root.gameObject });
		}
	}

	// Is the muscle a leg and if so, is it left or right?
	private bool IsLegMuscle(Muscle m, out bool isLeft)
	{
		isLeft = false;

		foreach (ConfigurableJoint j in leftLeg)
		{
			if (j == m.joint)
			{
				isLeft = true;
				return true;
			}
		}

		foreach (ConfigurableJoint j in rightLeg)
		{
			if (j == m.joint)
			{
				isLeft = false;
				return true;
			}
		}

		return false;
	}

	private bool IsDeadlyMuscle(Muscle m)
	{

		foreach (ConfigurableJoint j in deadly)
		{
			if (j == m.joint)
			{
				return true;
			}
		}


		return false;
	}
}
