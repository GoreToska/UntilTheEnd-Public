// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using UnityEngine;

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
		private void OnEnable()
		{
			conversationEvents.onConversationStart.AddListener(
				delegate
				{
					CameraStateManager.Instance.ActivateDialogueCamera();
					UIAnimations.Instance.DialogueFadeIn();
				});

			conversationEvents.onConversationEnd.AddListener(
				delegate
				{
					CameraStateManager.Instance.ActivateMainCamera();
					UIAnimations.Instance.DialogueFadeOut();
					StandardDialogueUI.Instance.ClearHistory();
				});
		}

		private void OnDisable()
		{
			conversationEvents.onConversationStart.RemoveAllListeners();
			conversationEvents.onConversationEnd.RemoveAllListeners();
		}
	}

}
