/*
 * Copyright (c) 2023 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;

namespace SafeExamBrowser.Settings.Monitoring
{
	/// <summary>
	/// Defines all settings for monitoring keyboard input.
	/// </summary>
	[Serializable]
	public class KeyboardSettings
	{
		/// <summary>
		/// Determines whether the user may use the ALT+ESC shortcut.
		/// </summary>
		public bool AllowAltEsc = true;

		/// <summary>
		/// Determines whether the user may use the ALT+F4 shortcut.
		/// </summary>
		public bool AllowAltF4 = true;

		/// <summary>
		/// Determines whether the user may use the ALT+TAB shortcut.
		/// </summary>
		public bool AllowAltTab = true;

		/// <summary>
		/// Determines whether the user may use the CTRL+C shortcut.
		/// </summary>
		public bool AllowCtrlC = true;

		/// <summary>
		/// Determines whether the user may use the CTRL+ESC shortcut.
		/// </summary>
		public bool AllowCtrlEsc = true;

		/// <summary>
		/// Determines whether the user may use the CTRL+V shortcut.
		/// </summary>
		public bool AllowCtrlV = true;

		/// <summary>
		/// Determines whether the user may use the CTRL+X shortcut.
		/// </summary>
		public bool AllowCtrlX = true;

		/// <summary>
		/// Determines whether the user may use the escape key.
		/// </summary>
		public bool AllowEsc = true;

		/// <summary>
		/// Determines whether the user may use the F1 key.
		/// </summary>
		public bool AllowF1 = true;

		/// <summary>
		/// Determines whether the user may use the F2 key.
		/// </summary>
		public bool AllowF2 = true;

		/// <summary>
		/// Determines whether the user may use the F3 key.
		/// </summary>
		public bool AllowF3 = true;

		/// <summary>
		/// Determines whether the user may use the F4 key.
		/// </summary>
		public bool AllowF4 = true;

		/// <summary>
		/// Determines whether the user may use the F5 key.
		/// </summary>
		public bool AllowF5 = true;

		/// <summary>
		/// Determines whether the user may use the F6 key.
		/// </summary>
		public bool AllowF6 = true;

		/// <summary>
		/// Determines whether the user may use the F7 key.
		/// </summary>
		public bool AllowF7 = true;

		/// <summary>
		/// Determines whether the user may use the F8 key.
		/// </summary>
		public bool AllowF8 = true;

		/// <summary>
		/// Determines whether the user may use the F9 key.
		/// </summary>
		public bool AllowF9 = true;

		/// <summary>
		/// Determines whether the user may use the F10 key.
		/// </summary>
		public bool AllowF10 = true;

		/// <summary>
		/// Determines whether the user may use the F11 key.
		/// </summary>
		public bool AllowF11 = true;

		/// <summary>
		/// Determines whether the user may use the F12 key.
		/// </summary>
		public bool AllowF12 = true;

		/// <summary>
		/// Determines whether the user may use the print screen key.
		/// </summary>
		public bool AllowPrintScreen = true;

		/// <summary>
		/// Determines whether the user may use the system key.
		/// </summary>
		public bool AllowSystemKey = true;
	}
}
