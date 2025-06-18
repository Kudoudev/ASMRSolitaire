using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSystem : SingletonMonoBehaviour < UpdateSystem > {

	List<RegisterSystem> OnUpdates = new List<RegisterSystem>();

	List<RegisterSystem> OnFixedUpdates = new List<RegisterSystem>();

	List<RegisterSystem> OnLatedUpdates = new List<RegisterSystem>();

	int UpdateCount = 0;

	int FixedCount = 0;

	int LatedCount = 0;

	public void RegisterUpdate(RegisterSystem param)
	{
		if ( !OnUpdates.Contains ( param ) )
		{
			OnUpdates.Add(param);

			UpdateCount++;
		} 
	}

	public void RemoveUpdate ( RegisterSystem param )
	{
		if (OnUpdates.Contains(param))
		{
			OnUpdates.Remove(param);

			UpdateCount--;
		}
	} 

	public void RegisterFixedUpdate(RegisterSystem param)
	{
		if (!OnFixedUpdates.Contains (param))
		{
			OnFixedUpdates.Add(param);

			FixedCount++;
		}
	}

	public void RemoveFixedUpdate(RegisterSystem param)
	{
		if (OnFixedUpdates.Contains(param))
		{
			OnFixedUpdates.Remove(param);

			FixedCount--;
		}
	}

	public void RegisterLatedUpdate(RegisterSystem param)
	{
		if (!OnLatedUpdates.Contains(param))
		{
			OnLatedUpdates.Add(param);

			LatedCount++;
		}
	}

	public void RemoveLatedUpdate(RegisterSystem param)
	{
		if (OnLatedUpdates.Contains(param))
		{
			OnLatedUpdates.Remove(param);

			LatedCount--;
		}
	}

	private void Update()
	{
		for ( int i = 0; i < UpdateCount; i++ )
		{
			OnUpdates[i].OnUpdate(); 
		}   
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < FixedCount; i++ )
		{
			OnFixedUpdates[i].OnFixedUpdate();
		}
	}

	private void LateUpdate()
	{
		for (int i = 0; i < LatedCount; i++)
		{
			OnLatedUpdates[i].OnLateUpdate();
		}
	}
}