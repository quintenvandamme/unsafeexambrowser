﻿/*
 * Copyright (c) 2023 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Net.Http;
using SafeExamBrowser.Logging.Contracts;
using SafeExamBrowser.Proctoring.ScreenProctoring.Imaging;
using SafeExamBrowser.Settings.Proctoring;

namespace SafeExamBrowser.Proctoring.ScreenProctoring.Service.Requests
{
	internal class ScreenShotRequest : Request
	{
		internal ScreenShotRequest(Api api, HttpClient httpClient, ILogger logger, Parser parser) : base(api, httpClient, logger, parser)
		{
		}

		internal bool TryExecute(ScreenShot screenShot, string sessionId, out string message)
		{
			var imageFormat = ("imageFormat", ToString(screenShot.Format));
			var timestamp = ("timestamp", DateTime.Now.ToUnixTimestamp().ToString());
			var url = api.ScreenShotEndpoint.Replace(Api.SESSION_ID, sessionId);
			var success = TryExecute(HttpMethod.Post, url, out var response, screenShot.Data, ContentType.OCTET_STREAM, Authorization, imageFormat, timestamp);

			message = response.ToLogString();

			return success;
		}

		private string ToString(ImageFormat format)
		{
			switch (format)
			{
				case ImageFormat.Bmp:
					return "bmp";
				case ImageFormat.Gif:
					return "gif";
				case ImageFormat.Jpg:
					return "jpg";
				case ImageFormat.Png:
					return "png";
				default:
					throw new NotImplementedException($"Image format {format} is not yet implemented!");
			}
		}
	}
}
