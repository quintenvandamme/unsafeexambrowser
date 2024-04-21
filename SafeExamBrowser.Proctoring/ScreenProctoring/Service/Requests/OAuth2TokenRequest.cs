﻿/*
 * Copyright (c) 2023 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Net.Http;
using SafeExamBrowser.Logging.Contracts;

namespace SafeExamBrowser.Proctoring.ScreenProctoring.Service.Requests
{
	internal class OAuth2TokenRequest : Request
	{
		internal OAuth2TokenRequest(Api api, HttpClient httpClient, ILogger logger, Parser parser) : base(api, httpClient, logger, parser)
		{
		}

		internal bool TryExecute(out string message)
		{
			return TryRetrieveOAuth2Token(out message);
		}
	}
}
