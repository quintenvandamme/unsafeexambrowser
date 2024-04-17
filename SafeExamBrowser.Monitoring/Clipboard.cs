﻿/*
 * Copyright (c) 2023 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Timers;
using SafeExamBrowser.Logging.Contracts;
using SafeExamBrowser.Monitoring.Contracts;
using SafeExamBrowser.Settings.Security;
using SafeExamBrowser.WindowsApi.Contracts;

namespace SafeExamBrowser.Monitoring
{
	public class Clipboard : IClipboard
	{
		private readonly ILogger logger;
		private readonly INativeMethods nativeMethods;
		private readonly Timer timer;

		private ClipboardPolicy policy;

		public Clipboard(ILogger logger, INativeMethods nativeMethods, int timeout_ms = 50)
		{
			this.logger = logger;
			this.nativeMethods = nativeMethods;
			this.timer = new Timer(timeout_ms);
		}

		public void Initialize(ClipboardPolicy policy)
		{
			this.policy = policy;

			if (policy != ClipboardPolicy.Allow)
			{
			}
			else
			{
				logger.Debug("Clipboard is allowed, not starting monitoring.");
			}

			logger.Info($"Initialized clipboard for policy '{policy}'.");
		}

		public void Terminate()
		{

		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
		}
	}
}
