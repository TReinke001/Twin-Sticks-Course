using UnityEngine;
using System.Collections;

public class ReplaySystem : MonoBehaviour {

	private const int bufferFrames = 10;
	private int lastFrame;
	private int frame;
	private int framesSkipped;

	private MyKeyFrame[] keyFrames = new MyKeyFrame[bufferFrames];

	private Rigidbody rigidBody;

	private GameManager gameManger;

	// Use this for initialization
	void Start () {
		//MyKeyFrame testKey = new MyKeyFrame (1.0f, Vector3.up, Quaternion.identity);
		rigidBody = GetComponent<Rigidbody>();
		gameManger= FindObjectOfType<GameManager>();
	}

	// Update is called once per frame
	void Update () {

		if(gameManger.recording == true){
		Record ();
		}else{
		PlayBack ();
		}
	}

	void Record ()
	{	rigidBody.isKinematic = false;
		frame = Mathf.Abs(Time.frameCount-framesSkipped )% bufferFrames;
		float time = Time.time;
		print ("writing frame " + frame);
		keyFrames [frame] = new MyKeyFrame (time, transform.position, transform.rotation);
		lastFrame = Time.frameCount-framesSkipped;
	}
	 
	void PlayBack(){
		rigidBody.isKinematic = true;

		if(lastFrame<bufferFrames){
			frame = Time.frameCount % lastFrame;
		}else{
			//framesSkipped =0;
		 	frame = Time.frameCount % bufferFrames ;
		}
		print ("Reading frame " + frame );
		transform.position = keyFrames[frame].pos;
		transform.rotation = keyFrames[frame].rot;
		framesSkipped=Time.frameCount;
	}
	

}

/// <summary>
/// A struction for storing time, position, and rotation.
/// </summary>
public struct MyKeyFrame{
	public float frametime;
	public Vector3 pos;
	public Quaternion rot;

	public  MyKeyFrame (float aTime, Vector3 aPos, Quaternion aRot){
	frametime=aTime;
	pos= aPos;
	rot=aRot;
	}

}