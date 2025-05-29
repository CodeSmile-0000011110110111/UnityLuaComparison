using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderLabelText : MonoBehaviour
{
	private Text m_Text;

	private void Awake() => m_Text = GetComponent<Text>();

	public void SetSliderValue(Slider slider) => SetTextValue((Int32)slider.value);
	private String SetTextValue(Int32 value) => m_Text.text = value.ToString();
}
