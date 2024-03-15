﻿// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using UnityEngine;
using Zenject;

namespace PixelCrushers.DialogueSystem.Wrappers
{

	/// <summary>
	/// This wrapper class keeps references intact if you switch between the 
	/// compiled assembly and source code versions of the original class.
	/// </summary>
	[HelpURL("https://pixelcrushers.com/dialogue_system/manual2x/html/dialogue_system_events.html")]
	[AddComponentMenu("Pixel Crushers/Dialogue System/Misc/Dialogue System Events")]
	public class DialogueSystemEvents : PixelCrushers.DialogueSystem.DialogueSystemEvents
	{
		[Inject] private CameraStateManager _cameraStateManager;
		[Inject] private UIAnimations _uiAnimations;
		[Inject] private StandardDialogueUI _standardDialogueUI;

		private void Awake()
		{
			conversationEvents.onConversationStart.AddListener(
				delegate
				{
					_cameraStateManager.ActivateDialogueCamera();
					_uiAnimations.DialogueFadeIn();
					InputReader.DisableAllInput();
				});

			conversationEvents.onConversationEnd.AddListener(
				delegate
				{
					_cameraStateManager.ActivateMainCamera();
					_uiAnimations.DialogueFadeOut();
					_standardDialogueUI.ClearHistory();
					InputReader.SwitchToGameControls();
				});
		}

		private void OnDisable()
		{
			conversationEvents.onConversationStart.RemoveAllListeners();
			conversationEvents.onConversationEnd.RemoveAllListeners();
		}
	}

}
