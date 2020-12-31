using UnityEngine;
using System.Collections;

//A very simple script to show test the RagdollHelper


public class RagdollScript : MonoBehaviour {

    private RagdollHelper helper;
	void Start () {

        helper = GetComponent<RagdollHelper>();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
            helper.Ragdolled = true;
		}

        if (Input.GetKeyDown(KeyCode.K))
        {
            helper.Ragdolled = false;
        }
	}
}
