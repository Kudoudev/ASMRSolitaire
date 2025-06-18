using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterSystem : MonoBehaviour {

	[Tooltip ("Register System Update.")]
	public bool IsRegisterUpdateSystem = true;

	public bool IsRegisterFixedUpdateSystem = false;

	public bool IsReigisterLateUpdateSystem = false;

	public virtual void OnEnable()
	{
		if (IsRegisterUpdateSystem)
		{
			UpdateSystem.Instance.RegisterUpdate(this);

			LogGame.DebugLog(string.Format("[System Register] Register Listener Update Completed!"));
		}

		if (IsRegisterFixedUpdateSystem)
		{
			UpdateSystem.Instance.RegisterFixedUpdate(this);

			LogGame.DebugLog(string.Format("[System Register] Register Listener Fixed Update Completed!"));
		}

		if (IsReigisterLateUpdateSystem)
		{
			UpdateSystem.Instance.RegisterLatedUpdate(this);

			LogGame.DebugLog(string.Format("[System Register] Register Listener Lated Update Completed!"));
		}
	}

	public virtual void OnDisable()
	{
		if (UpdateSystem.Instance == null)
		{
			return;
		}

		if (IsRegisterUpdateSystem)
		{
			UpdateSystem.Instance.RemoveUpdate(this);

			LogGame.DebugLog(string.Format("[System Register] Remove Listener Update Completed!"));
		}

		if (IsRegisterFixedUpdateSystem)
		{
			UpdateSystem.Instance.RemoveFixedUpdate(this);

			LogGame.DebugLog(string.Format("[System Register] Remove Listener Fixed Update Completed!"));
		}

		if (IsReigisterLateUpdateSystem)
		{
			UpdateSystem.Instance.RemoveLatedUpdate(this);

			LogGame.DebugLog(string.Format("[System Register] Remove Listener Lated Update Completed!"));
		}
	}

	public virtual void OnUpdate() { }

	public virtual void OnFixedUpdate() { }

	public virtual void OnLateUpdate() { }
}