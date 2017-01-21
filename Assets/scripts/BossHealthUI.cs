using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour 
{
	[SerializeField]
	private Slider hpSlider;

	public void SetHealthBarVisible(bool visible)
	{
		hpSlider.gameObject.SetActive(visible);
	}

	/// <summary>
	/// Set the slider value between 0 and 1
	/// </summary>
	public void SetHealthBarValue(float value)
	{
		hpSlider.value = value;
	}
}
