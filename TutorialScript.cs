using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour {

    public Animator Finger, Trajectory, BounceText, AvoidObstaclesText, EndPointText;

	void Start () {
        StartCoroutine(TutorialAnimation());
	}
	
	IEnumerator TutorialAnimation() {

        yield return new WaitForSeconds(4.5f);

        Finger.SetTrigger("Finger");

        yield return new WaitForSeconds(0.85f);

        Trajectory.SetTrigger("Trajectory");

        yield return new WaitForSeconds(0.7f);

        BounceText.SetTrigger("TextTrigger");

        yield return new WaitForSeconds(1f);

        AvoidObstaclesText.SetTrigger("TextTrigger");

        yield return new WaitForSeconds(1f);

        EndPointText.SetTrigger("TextTrigger");

    }
}
