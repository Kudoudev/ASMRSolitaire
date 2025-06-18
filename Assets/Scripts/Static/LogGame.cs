using UnityEngine;
using System.Collections;

/// <summary>
/// Log game.
/// </summary>
public static class LogGame {

	/// <summary>
	/// Debugs the log.
	/// </summary>
	/// <param name="message">Message.</param>
	[ System.Diagnostics.Conditional ("LOG_ENABLE")]
	public static void DebugLog (string message)
	{
		Debug.Log(message);        
	}

	/// <summary>
	/// Debugs the log warning.
	/// </summary>
	/// <param name="message">Message.</param>
	[ System.Diagnostics.Conditional ("LOG_ENABLE")]
	public static void DebugLogWarning(string message)
	{
		Debug.LogWarning(message);    
	}

	/// <summary>
	/// Debugs the log error.
	/// </summary>
	/// <param name="message">Message.</param>
	[ System.Diagnostics.Conditional ("LOG_ENABLE")]
	public static void DebugLogError(string message)
	{
		Debug.LogError(message);
	}

	/// <summary>
	/// Debugs the log error.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="context">Context.</param>
	[ System.Diagnostics.Conditional ("LOG_ENABLE")]
	public static void DebugLogError(string message , Object context)
	{
		Debug.LogError(message, context);
	}

}