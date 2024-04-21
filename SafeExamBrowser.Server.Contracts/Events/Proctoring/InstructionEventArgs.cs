﻿/*
 * Copyright (c) 2023 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace SafeExamBrowser.Server.Contracts.Events.Proctoring
{
	/// <summary>
	/// Defines all parameters for a proctoring instruction received by the <see cref="IServerProxy"/>.
	/// </summary>
	public abstract class InstructionEventArgs
	{
		public InstructionMethod Method { get; set; }
	}
}
