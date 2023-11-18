/*
 * Copyright (c) 2023 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Linq;
using SafeExamBrowser.Logging.Contracts;
using SafeExamBrowser.SystemComponents.Contracts;
using SafeExamBrowser.SystemComponents.Contracts.Registry;

namespace SafeExamBrowser.SystemComponents
{
	public class VirtualMachineDetector : IVirtualMachineDetector
	{	
		private readonly ILogger logger;
		private readonly IRegistry registry;
		private readonly ISystemInfo systemInfo;

		public VirtualMachineDetector(ILogger logger, IRegistry registry, ISystemInfo systemInfo)
		{
			this.logger = logger;
			this.registry = registry;
			this.systemInfo = systemInfo;
		}

		public bool IsVirtualMachine()
		{
			return false;
		}

		private bool HasVirtualDevice()
		{
			return false;
		}

		private bool HasVirtualMacAddress()
		{
			return false;
		}

		private bool IsVirtualCpu()
		{
			return false;
		}

		private bool IsVirtualRegistry()
		{
			return false;
		}

		private bool IsVirtualSystem(string biosInfo, string manufacturer, string model)
		{
			return false;
		}

		private bool HasHistoricVirtualMachineHardwareConfiguration()
		{
			return false;
		}

		private bool HasLocalVirtualMachineDeviceCache()
		{
			return false;
		}
	}
}
