using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TypewriterText : MonoBehaviour 
{
	[SerializeField]
	private float fastCharacterDelay = 0.01f;

	[SerializeField]
	private float slowCharacterDelay = 0.1f;

	[SerializeField]
	[TextArea(5,10)]
	private string text;

	[SerializeField]
	private float enterPromptDuration = 0.5f;

	private float timeLastCharacterInserted;

	private float currentCharacterDelay;

	private int currentCharIndex = 0;

	private Text uiText;

	private bool waitingForKeyPress = false;
	private string oldText;
	private bool promptVisible = false;
	private float timePromptLastShown;


	void Awake()
	{
		uiText = GetComponent<Text>();
		uiText.text = string.Empty;
	}

	void Start()
	{
		currentCharacterDelay = fastCharacterDelay;
	}

	void Update()
	{
		if (waitingForKeyPress)
		{
			if (Time.time - timePromptLastShown >= enterPromptDuration)
			{
				// Toggle prompt visibility
				if (promptVisible)
				{
					uiText.text = oldText;
				}
				else
				{
					uiText.text = oldText + "\n[press any key]";
				}
				promptVisible = !promptVisible;
				timePromptLastShown = Time.time;
			}

			if (Input.anyKeyDown)
			{
				waitingForKeyPress = false;
				uiText.text = string.Empty;
			}
			return;
		}

		if (Time.time >= timeLastCharacterInserted + currentCharacterDelay
			&& text.Length > currentCharIndex)
		{
			// Character after '\' have special meaning
			var currentChar = text[currentCharIndex];
			if (currentChar == '\\')
			{
				currentCharIndex++;
				currentChar = text[currentCharIndex];
				switch (currentChar)
				{
					case 's': // s = slow
						currentCharacterDelay = slowCharacterDelay;
						break;
					case 'f': // f = fast
						currentCharacterDelay = fastCharacterDelay;
						break;
					case 'e': // e = prompt (was Enter, now any key)
						waitingForKeyPress = true;
						oldText = uiText.text;
						break;
					case 'b': // b = begin!
						BeginGame();
						break;
					case '\\': // actually insert a backslash
						currentCharIndex--;
						break;
					default: break;
				}

				currentCharIndex++;
				currentChar = text[currentCharIndex];
			}
			uiText.text = uiText.text + currentChar;
			timeLastCharacterInserted = Time.time;

			currentCharIndex++;
		}
	}

	private void BeginGame()
	{
		
	}
}
