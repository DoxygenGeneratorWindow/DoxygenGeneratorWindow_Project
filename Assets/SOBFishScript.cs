using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// SOB fish script. It's class For AI of fish (not hight AI)
/// </summary>
public class SOBFishScript : MonoBehaviour {

	[Header("Timer Randomize")]
	/// <summary>
	/// The minimum timer.
	/// </summary>
	public float MinTimer = 5.0f;
	/// <summary>
	/// The max timer.
	/// </summary>
	public float MaxTimer = 10.0f;
	[Header("Fake Target OR Target Track")]
	/// <summary>
	/// The color target.
	/// </summary>
	public Color ColorTarget = Color.red;
	public float FakeTargetRadius = 100.0f;
	/// <summary>
	/// The target object.
	/// </summary>
	public GameObject TargetObject;
	[Header("Position Force")]
	/// <summary>
	/// The force position to Y value.
	/// </summary>
	public bool ForcePositionY = false; 
	/// <summary>
	/// The color gizmos position.
	/// </summary>
	public Color ColorGizmosPosition = Color.gray;
	/// <summary>
	/// The minimum position y.
	/// </summary>
	public float MinPositionY = 0.0F;
	/// <summary>
	/// The max position y.
	/// </summary>
	public float MaxPositionY = 10.0F;
	/// <summary>
	/// The agent.
	/// </summary>
	private NavMeshAgent agent;
	/// <summary>
	/// The delta timer.
	/// </summary>
	private float DeltaTimer;
	/// <summary>
	/// The animation.
	/// </summary>
	public Animator Anim;
	/// <summary>
	/// The target position.
	/// </summary>
	private Vector3 TargetPosition;
	// Use this for initialization
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		//Debug.Log ("SOBFishScript Start");
		agent = GetComponent<NavMeshAgent> ();
		agent.SetDestination (transform.position);
		//ColorTarget = new Color (1.00F, 0.00F, 0.00F, 0.50F);
	}
	/// <summary>
	/// Raises the enable event.
	/// Use this for initialization
	/// </summary>
	void OnEnable () {
		//Debug.Log ("SOBFishScript OnEnable");
//		agent = GetComponent<NavMeshAgent> ();
	}
	/// <summary>
	/// Update this instance.
	/// Update is called once per frame
	/// </summary>
	void Update () {
		//Debug.Log ("SOBFishScript Update timer = " + timer);
		DeltaTimer -= Time.deltaTime;
		if (ForcePositionY == true) {
			if (transform.position.y < MinPositionY) {
				transform.position = new Vector3 (transform.position.x, MinPositionY, transform.position.z);
			} else if (transform.position.y > MaxPositionY) {
				transform.position = new Vector3 (transform.position.x, MaxPositionY, transform.position.z);
			}
		}
		if (DeltaTimer <=0) {
			if (agent != null) {
				if (TargetObject != null) {
					TargetPosition = TargetObject.transform.position;
					agent.SetDestination (TargetObject.transform.position);
				} else {
					Vector3 newPos = RandomNavSphere(transform.position, FakeTargetRadius, -1);
					TargetPosition = newPos;
					agent.SetDestination (newPos);
				}
			}
			DeltaTimer = Random.Range(MinTimer,MaxTimer);
		}
		if (Anim != null) {
//		Debug.Log ("agent.desiredVelocity = " + agent.desiredVelocity.magnitude);
//		Debug.Log ("agent.velocity = " + agent.velocity.magnitude);
			Anim.SetFloat ("Speed", agent.velocity.magnitude);
		}
	}
	/// <summary>
	/// Randoms the next position in nav sphere.
	/// </summary>
	/// <returns>The nav sphere.</returns>
	/// <param name="origin">Origin.</param>
	/// <param name="dist">Dist.</param>
	/// <param name="layermask">Layermask.</param>
	public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
		//Debug.Log ("SOBFishScript RandomNavSphere");
		Vector3 randDirection = Random.insideUnitSphere * dist;

		randDirection += origin;

		NavMeshHit navHit;

		NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);

		return navHit.position;
	}


	#if UNITY_EDITOR
	/// <summary>
	/// Raises the draw gizmos event.
	/// </summary>
	void OnDrawGizmos ()
	{
		Handles.color = ColorTarget;
		if (TargetObject == null) {
			Handles.DrawWireDisc (transform.position, Vector3.up, FakeTargetRadius);
		} else {
			Handles.DrawWireDisc (TargetObject.transform.position, Vector3.up, 1.0f);
			Handles.DrawLine (transform.position, TargetObject.transform.position);
		}
	}
	/// <summary>
	/// Raises the draw gizmos selected event.
	/// </summary>
	void OnDrawGizmosSelected ()
	{
		Handles.color = ColorTarget;
		if (TargetObject == null) {
			Handles.DrawSolidDisc (transform.position, Vector3.up, FakeTargetRadius);
		} else {
			Handles.DrawSolidDisc (TargetObject.transform.position, Vector3.up, 1.0f);
			Handles.DrawSolidDisc (transform.position, Vector3.up, 1.0f);
			Handles.DrawLine (transform.position, TargetObject.transform.position);
		}
		if (ForcePositionY) {
			Handles.color = ColorGizmosPosition;
			Handles.DrawSolidDisc (new Vector3 (transform.position.x, MinPositionY, transform.position.z), Vector3.up, 0.5f);
			Handles.DrawSolidDisc (new Vector3 (transform.position.x, MaxPositionY, transform.position.z), Vector3.up, 0.5f);
		}

		Handles.DrawSolidDisc (TargetPosition, Vector3.up, 1.0f);
		Handles.DrawLine (transform.position, TargetPosition);
	}
	#endif
}
