using UnityEngine;
using System.Collections;

public class StairDismount : MonoBehaviour {
	//Declare a member variables for distributing the impacts over several frames
	float impactEndTime=0;
	Rigidbody impactTarget=null;
	Vector3 impact;
	//Current score
	public int score;
	//A prefab for displaying points (floats up, fades out, instantiated by the RagdollPartScript)
	public GameObject scoreTextTemplate;
    
    // Use this for initialization
	void Start () {
	
		/*//Get all the rigid bodies that belong to the ragdoll
		Rigidbody[] rigidBodies=GetComponentsInChildren<Rigidbody>();
		
		//Add the RagdollPartScript to all the gameobjects that also have the a rigid body
		foreach (Rigidbody body in rigidBodies)
		{
			RagdollPartScript rps=body.gameObject.AddComponent<RagdollPartScript>();
			
			//Set the scripts mainScript reference so that it can access
			//the score and scoreTextTemplate member variables above
			rps.mainScript=this;
		}*/

    }
	
	// Update is called once per frame
	void Update () {
		
		//Pressing space makes the character get up, assuming that the character root has
		//a RagdollHelper script
		if (Input.GetKeyDown(KeyCode.Space))
		{
			RagdollHelper helper=GetComponent<RagdollHelper>();
			helper.ragdolled=false;
        }	
		
		//Check if we need to apply an impact
		/*if (Time.time<impactEndTime)
		{
			impactTarget.AddForce(impact,ForceMode.VelocityChange);
		}*/
	}

}
