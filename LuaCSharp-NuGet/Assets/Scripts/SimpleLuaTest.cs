using Lua;
using Lua.Runtime;
using System;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SimpleLuaTest : MonoBehaviour
{
	private const String CallLuaFunction = "CallLuaFunction";
	private const String CallCSharpMethod = "CallCSharpMethod";
	[SerializeField] private Int32 m_LuaFunctionCallIterations = 5000;

	private LuaState m_LuaState;

	private ProfilerMarker m_LookupMarker;
	private ProfilerMarker m_CallLuaMarker;
	private ProfilerMarker m_CallCSharpMarker;


	private void Start()
	{
		var script = $"function {CallLuaFunction}()\n" +
		             "end\n" +
		             $"function {CallCSharpMethod}()\n" +
		             "UsedByLua.CSharpMethod()\n" +
		             "end\n";

		m_LuaState = LuaState.Create();
		m_LuaState.DoStringAsync(script);
		m_LuaState.Environment["UsedByLua"] = new LuaValue(new UsedByLua());

		m_LookupMarker = new ProfilerMarker("FunctionLookup");
		m_CallLuaMarker = new ProfilerMarker(CallLuaFunction);
		m_CallCSharpMarker = new ProfilerMarker(CallCSharpMethod);
	}

	private void Update()
	{
		using (m_CallLuaMarker.Auto())
		{
			for (var i = 0; i < m_LuaFunctionCallIterations; i++)
			{
				m_LookupMarker.Begin();
				var func = m_LuaState.Environment[CallLuaFunction];
				var luaFunction = func.Read<LuaFunction>();
				var thread = m_LuaState.TopLevelAccess;
				m_LookupMarker.End();

				thread.RunAsync(luaFunction);
			}
		}

		using (m_CallCSharpMarker.Auto())
		{
			for (var i = 0; i < m_LuaFunctionCallIterations; i++)
			{
				m_LookupMarker.Begin();
				var func = m_LuaState.Environment[CallCSharpMethod];
				var luaFunction = func.Read<LuaFunction>();
				var thread = m_LuaState.TopLevelAccess;
				m_LookupMarker.End();

				thread.RunAsync(luaFunction);
			}
		}
	}

	public void SetIterations(Slider slider) => m_LuaFunctionCallIterations = (Int32)slider.value;
}

[LuaObject]
public partial class UsedByLua
{
	[LuaMember]
	public Boolean CSharpMethod()
	{
		Debug.Log("called");
		return true;
	}
}
