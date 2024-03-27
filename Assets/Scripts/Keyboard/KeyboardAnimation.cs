using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyboardAnimation : MonoBehaviour
{
	public GameObject Keyboard;

	public void ResetButton ()
	{
		Animator animator = Keyboard.GetComponent<Animator>();
		animator.SetBool("open", false);
		animator.SetBool("other", false);
		animator.SetBool("close_eng_small", false);
		animator.SetBool("close_eng_caps", false);
		animator.SetBool("close_eng_shift", false);
		animator.SetBool("close_fr_small", false);
		animator.SetBool("close_fr_caps", false);
		animator.SetBool("close_fr_shift", false);
	}

	public void Other ()
	{
		Animator animator = Keyboard.GetComponent<Animator>();
		animator.SetBool("other", true);
		animator.SetBool("open", false);
	}
	
	public void OpenEngSmall ()
	{
		Animator animator = Keyboard.GetComponent<Animator>();
		animator.SetBool("open", true);
	}
	
	public void CloseEngSmall ()
	{
		Animator animator = Keyboard.GetComponent<Animator>();
		animator.SetBool("close_eng_small", true);
		animator.SetBool("other", false);
		StartCoroutine(Delay());
		animator.SetBool("close_eng_small", false);
	}
	
	public void CloseEngCaps ()
	{
		Animator animator = Keyboard.GetComponent<Animator>();
		animator.SetBool("close_eng_caps", true);
		animator.SetBool("other", false);
		StartCoroutine(Delay());
		animator.SetBool("close_eng_caps", false);
	}
	
	public void CloseEngShift ()
	{
		Animator animator = Keyboard.GetComponent<Animator>();
		animator.SetBool("close_eng_shift", true);
		animator.SetBool("other", false);
		StartCoroutine(Delay());
		animator.SetBool("close_eng_shift", false);


	}
	
	public void CloseFrfSmall ()
	{
		Animator animator = Keyboard.GetComponent<Animator>();
		animator.SetBool("close_fr_small", true);
	}
	
	public void CloseFrCaps ()
	{
		Animator animator = Keyboard.GetComponent<Animator>();
		animator.SetBool("close_fr_caps", true);
	}
	
	public void CloseFrShift ()
	{
		Animator animator = Keyboard.GetComponent<Animator>();
		animator.SetBool("close_fr_shift", true);
	}
	
	private IEnumerator Delay ()
	{
		yield return new WaitForSeconds(5);
	}
}