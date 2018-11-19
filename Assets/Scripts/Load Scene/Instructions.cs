using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour {

	public GameObject instructions;

	public void ShowInstructions()
	{
		instructions.SetActive(true);
	}

	public void HideInstructions()
	{
		instructions.SetActive(false);
	}
}
