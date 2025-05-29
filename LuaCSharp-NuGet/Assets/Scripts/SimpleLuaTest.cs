using Lua;
using Lua.Runtime;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SimpleLuaTest : MonoBehaviour
{
	private const String LuaFunctionName = "TestLuaFunction";
	[SerializeField] private Int32 m_LuaFunctionCallIterations = 5000;

	private LuaState m_LuaState;

	private void Start()
	{
		var script = $"function {LuaFunctionName}()" +
		             "end";

		m_LuaState = LuaState.Create();
		m_LuaState.DoStringAsync(script);
	}

	private void Update()
	{
		for (var i = 0; i < m_LuaFunctionCallIterations; i++)
		{
			var func = m_LuaState.Environment[LuaFunctionName];
			var luaFunction = func.Read<LuaFunction>();
			m_LuaState.TopLevelAccess.RunAsync(luaFunction);
		}
	}

	public void SetIterations(Slider slider)
	{
		m_LuaFunctionCallIterations = (int)slider.value;
	}
}
