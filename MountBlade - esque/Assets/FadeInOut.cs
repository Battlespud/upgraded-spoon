using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour {

	public bool Fade;
	public bool Go = true;

	void Update()
	{
		if (!Fade && Go)
		{
			StartCoroutine(FadeTextToFullAlpha(1.5f, GetComponent<Text>()));
		}
		if (Fade && Go)
		{
			StartCoroutine(FadeTextToZeroAlpha(1.5f, GetComponent<Text>()));
		}
	}



	public IEnumerator FadeTextToFullAlpha(float t, Text i)
	{
		Go = false;
		i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
		while (i.color.a < 1.0f)
		{
			i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
			yield return null;
		}
		Fade = true;
		Go = true;
	}

	public IEnumerator FadeTextToZeroAlpha(float t, Text i)
	{
		Go = false;
		i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
		while (i.color.a > 0.0f)
		{
			i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
			yield return null;
		}
		Fade = false;
		Go = true;
	}

}
