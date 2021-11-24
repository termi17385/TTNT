using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Used to convert the time
/// values in the background </summary>
public static class TimeUtils
{
	public static int MinutesToSeconds(float _mins)
	{
		return Mathf.RoundToInt(_mins * 60);
	}

	public static float SecondsToMinutes(float _seconds)
	{
		return _seconds / 60;
	}
}
