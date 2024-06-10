using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameFrame
{
	public enum LogLevel
    {
		None,
		Error,
		Warning,
		Normal,
    }

    public static class Log
	{
		public static LogLevel LogLevel = LogLevel.Normal;

		public static void e(string content)
		{
			if (LogLevel >= LogLevel.Error)
			{
				Debug.LogError(content);
			}
		}

		public static void w(string content)
		{
			if (LogLevel >= LogLevel.Warning)
			{
				Debug.LogWarning(content);
			}
		}

		public static void i(string content)
		{
			if (LogLevel >= LogLevel.Normal)
			{
				Debug.Log(content);
			}
		}
	}
	
}