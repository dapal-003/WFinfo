﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WFInfoCS
{
    class Win32
	{
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetWindowRect(HandleRef hwnd, out RECT lpRect);

		[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
		public static extern uint GetWindowLongPtr(HandleRef hwnd, int nIndex);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetForegroundWindow();

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
	}
}
