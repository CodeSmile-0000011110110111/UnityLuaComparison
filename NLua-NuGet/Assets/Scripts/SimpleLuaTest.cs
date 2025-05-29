using NLua;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SimpleLuaTest : MonoBehaviour
{
	private const String LuaFunctionName = "TestLuaFunction";
	[SerializeField] private Int32 m_LuaFunctionCallIterations = 5000;

	private Lua m_LuaState;

	private void Start()
	{
		var script = $"function {LuaFunctionName}()" +
		             "end";

		m_LuaState = new Lua(true);
		m_LuaState.DoString(script);
	}

	private void Update()
	{
		for (var i = 0; i < m_LuaFunctionCallIterations; i++)
		{
			var func = m_LuaState[LuaFunctionName];
			var luaFunction = (LuaFunction)func;
			luaFunction.Call();
		}
	}

	public void SetIterations(Slider slider)
	{
		m_LuaFunctionCallIterations = (int)slider.value;
	}
}
