using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TypewriterText : MonoBehaviour 
{
	[SerializeField]
	private float characterDelay = 0.1f;

	[SerializeField]
	[TextArea(5,10)]
	private string text;

	private float timeLastCharacterInserted;

	private Text uiText;

	void Awake()
	{
		uiText = GetComponent<Text>();
		uiText.text = string.Empty;
	}

	void Update()
	{
		if (Time.time >= timeLastCharacterInserted + characterDelay
			&& text.Length > uiText.text.Length)
		{
			var index = uiText.text.Length;
			uiText.text = uiText.text + text[index];
			timeLastCharacterInserted = Time.time;
		}
	}
}
