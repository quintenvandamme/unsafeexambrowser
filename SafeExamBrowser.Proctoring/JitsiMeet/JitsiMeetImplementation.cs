﻿/*
 * Copyright (c) 2023 ETH Zürich, Educational Development and Technology (LET)
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using Microsoft.Web.WebView2.Wpf;
using SafeExamBrowser.Configuration.Contracts;
using SafeExamBrowser.Core.Contracts.Notifications.Events;
using SafeExamBrowser.Core.Contracts.Resources.Icons;
using SafeExamBrowser.I18n.Contracts;
using SafeExamBrowser.Logging.Contracts;
using SafeExamBrowser.Server.Contracts.Events;
using SafeExamBrowser.Settings.Proctoring;
using SafeExamBrowser.SystemComponents.Contracts;
using SafeExamBrowser.UserInterface.Contracts;
using SafeExamBrowser.UserInterface.Contracts.Proctoring;

namespace SafeExamBrowser.Proctoring.JitsiMeet
{
	internal class JitsiMeetImplementation : ProctoringImplementation
	{
		private readonly AppConfig appConfig;
		private readonly IFileSystem fileSystem;
		private readonly IModuleLogger logger;
		private readonly ProctoringSettings settings;
		private readonly IText text;
		private readonly IUserInterfaceFactory uiFactory;

		private ProctoringControl control;
		private string filePath;
		private WindowVisibility initialVisibility;
		private IProctoringWindow window;

		internal override string Name => nameof(JitsiMeet);

		public override string Tooltip { get; protected set; }
		public override IconResource IconResource { get; protected set; }

		public override event NotificationChangedEventHandler NotificationChanged;

		internal JitsiMeetImplementation(
			AppConfig appConfig,
			IFileSystem fileSystem,
			IModuleLogger logger,
			ProctoringSettings settings,
			IText text,
			IUserInterfaceFactory uiFactory)
		{
			this.appConfig = appConfig;
			this.fileSystem = fileSystem;
			this.logger = logger;
			this.settings = settings;
			this.text = text;
			this.uiFactory = uiFactory;

			IconResource = new XamlIconResource { Uri = new Uri("pack://application:,,,/SafeExamBrowser.UserInterface.Desktop;component/Images/ProctoringNotification_Inactive.xaml") };
			Tooltip = text.Get(TextKey.Notification_ProctoringInactiveTooltip);
		}

		public override void Activate()
		{
			if (settings.WindowVisibility == WindowVisibility.Visible)
			{
				window?.BringToForeground();
			}
			else if (settings.WindowVisibility == WindowVisibility.AllowToHide || settings.WindowVisibility == WindowVisibility.AllowToShow)
			{
				window?.Toggle();
			}
		}

		internal override void Initialize()
		{
			var start = true;

			initialVisibility = settings.WindowVisibility;
			settings.JitsiMeet.ServerUrl = Sanitize(settings.JitsiMeet.ServerUrl);

			start &= !string.IsNullOrWhiteSpace(settings.JitsiMeet.RoomName);
			start &= !string.IsNullOrWhiteSpace(settings.JitsiMeet.ServerUrl);

			logger.Info("Initialized proctoring.");

			if (start)
			{
				StartProctoring();
			}
		}

		internal override void ProctoringConfigurationReceived(bool allowChat, bool receiveAudio, bool receiveVideo)
		{
			logger.Info("Proctoring configuration received.");

			settings.JitsiMeet.AllowChat = allowChat;
			settings.JitsiMeet.ReceiveAudio = receiveAudio;
			settings.JitsiMeet.ReceiveVideo = receiveVideo;

			if (allowChat || receiveVideo)
			{
				settings.WindowVisibility = WindowVisibility.AllowToHide;
			}
			else
			{
				settings.WindowVisibility = initialVisibility;
			}

			StopProctoring();
			StartProctoring();

			logger.Info($"Successfully updated configuration: {nameof(allowChat)}={allowChat}, {nameof(receiveAudio)}={receiveAudio}, {nameof(receiveVideo)}={receiveVideo}.");
		}

		internal override void ProctoringInstructionReceived(ProctoringInstructionEventArgs args)
		{
			logger.Info("Proctoring instruction received.");

			settings.JitsiMeet.RoomName = args.JitsiMeetRoomName;
			settings.JitsiMeet.ServerUrl = args.JitsiMeetServerUrl;
			settings.JitsiMeet.Token = args.JitsiMeetToken;

			StopProctoring();
			StartProctoring();

			logger.Info("Successfully processed instruction.");
		}

		internal override void StartProctoring()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				try
				{
					var content = LoadContent(settings);

					filePath = Path.Combine(appConfig.TemporaryDirectory, $"{Path.GetRandomFileName()}_index.html");
					fileSystem.Save(content, filePath);

					control = new ProctoringControl(logger.CloneFor(nameof(ProctoringControl)), settings);
					control.CreationProperties = new CoreWebView2CreationProperties { UserDataFolder = appConfig.TemporaryDirectory };
					control.EnsureCoreWebView2Async().ContinueWith(_ =>
					{
						control.Dispatcher.Invoke(() =>
						{
							control.CoreWebView2.Navigate(filePath);
						});
					});

					window = uiFactory.CreateProctoringWindow(control);
					window.SetTitle(settings.JitsiMeet.Enabled ? settings.JitsiMeet.Subject : "");
					window.Show();

					if (settings.WindowVisibility == WindowVisibility.AllowToShow || settings.WindowVisibility == WindowVisibility.Hidden)
					{
						window.Hide();
					}

					ShowNotificationActive();

					logger.Info("Started proctoring.");
				}
				catch (Exception e)
				{
					logger.Error($"Failed to start proctoring! Reason: {e.Message}", e);
				}
			});
		}

		internal override void StopProctoring()
		{
			if (control != default && window != default)
			{
				control.Dispatcher.Invoke(() =>
				{
					control.ExecuteScriptAsync("api.executeCommand('hangup'); api.dispose();");

					Thread.Sleep(2000);

					window.Close();
					control = default;
					window = default;
					fileSystem.Delete(filePath);

					ShowNotificationInactive();

					logger.Info("Stopped proctoring.");
				});
			}
		}

		internal override void Terminate()
		{
			logger.Info("Terminated proctoring.");
		}

		private string LoadContent(ProctoringSettings settings)
		{
			var assembly = Assembly.GetAssembly(typeof(ProctoringController));
			var path = $"{typeof(ProctoringController).Namespace}.JitsiMeet.index.html";

			using (var stream = assembly.GetManifestResourceStream(path))
			using (var reader = new StreamReader(stream))
			{
				var html = reader.ReadToEnd();

				html = html.Replace("%%_ALLOW_CHAT_%%", settings.JitsiMeet.AllowChat ? "chat" : "");
				html = html.Replace("%%_ALLOW_CLOSED_CAPTIONS_%%", settings.JitsiMeet.AllowClosedCaptions ? "closedcaptions" : "");
				html = html.Replace("%%_ALLOW_RAISE_HAND_%%", settings.JitsiMeet.AllowRaiseHand ? "raisehand" : "");
				html = html.Replace("%%_ALLOW_RECORDING_%%", settings.JitsiMeet.AllowRecording ? "recording" : "");
				html = html.Replace("%%_ALLOW_TILE_VIEW", settings.JitsiMeet.AllowTileView ? "tileview" : "");
				html = html.Replace("'%_AUDIO_MUTED_%'", settings.JitsiMeet.AudioMuted && settings.WindowVisibility != WindowVisibility.Hidden ? "true" : "false");
				html = html.Replace("'%_AUDIO_ONLY_%'", settings.JitsiMeet.AudioOnly ? "true" : "false");
				html = html.Replace("'%_VIDEO_MUTED_%'", settings.JitsiMeet.VideoMuted && settings.WindowVisibility != WindowVisibility.Hidden ? "true" : "false");

				return html;
			}
		}

		private string Sanitize(string serverUrl)
		{
			return serverUrl?.Replace($"{Uri.UriSchemeHttp}{Uri.SchemeDelimiter}", "").Replace($"{Uri.UriSchemeHttps}{Uri.SchemeDelimiter}", "");
		}

		private void ShowNotificationActive()
		{
			IconResource = new XamlIconResource { Uri = new Uri("pack://application:,,,/SafeExamBrowser.UserInterface.Desktop;component/Images/ProctoringNotification_Active.xaml") };
			Tooltip = text.Get(TextKey.Notification_ProctoringActiveTooltip);
			NotificationChanged?.Invoke();
		}

		private void ShowNotificationInactive()
		{
			IconResource = new XamlIconResource { Uri = new Uri("pack://application:,,,/SafeExamBrowser.UserInterface.Desktop;component/Images/ProctoringNotification_Inactive.xaml") };
			Tooltip = text.Get(TextKey.Notification_ProctoringInactiveTooltip);
			NotificationChanged?.Invoke();
		}
	}
}
