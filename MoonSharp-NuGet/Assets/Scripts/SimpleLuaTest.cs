using MoonSharp.Interpreter;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SimpleLuaTest : MonoBehaviour
{
	private const String LuaFunctionName = "TestLuaFunction";
	[SerializeField] private Int32 m_LuaFunctionCallIterations = 5000;

	private Script m_LuaState;

	private void Start()
	{
		var script = $"function {LuaFunctionName}()" +
		             "end";

		m_LuaState = new Script();
		m_LuaState.DoString(script);
	}

	private void Update()
	{
		for (var i = 0; i < m_LuaFunctionCallIterations; i++)
			m_LuaState.Call(m_LuaState.Globals[LuaFunctionName]);
	}

	public void SetIterations(Slider slider) => m_LuaFunctionCallIterations = (Int32)slider.value;
}
