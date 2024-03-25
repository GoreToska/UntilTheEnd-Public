// Copyright (c) Pixel Crushers. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrushers.DialogueSystem.Wrappers
{

	/// <summary>
	/// This wrapper class keeps references intact if you switch between the
	/// compiled assembly and source code versions of the original class.
	/// </summary>
	[HelpURL("http://www.pixelcrushers.com/dialogue_system/manual2x/html/standard_dialogue_u_i.html")]
	[AddComponentMenu("Pixel Crushers/Dialogue System/UI/Standard UI/Dialogue/Standard Dialogue UI")]
	public class StandardDialogueUI : PixelCrushers.DialogueSystem.StandardDialogueUI
	{
		[Header("Conversation History")]
		[SerializeField] private GameObject _conversationDisplay;
		[SerializeField] private GameObject _phraseExample;

		[SerializeField] private List<GameObject> _additionalUI;

		[Header("Autoscroll")]
		[SerializeField] private ScrollRect _scrollRect;
		[SerializeField] private Scrollbar _scrollBar;
		[SerializeField] private float _scrollSpeed = 1f;
		private Coroutine _scrollCoroutine = null;

		private List<GameObject> _phrases = new List<GameObject>();
		private Subtitle _previousSubtitle;
		private float _delta = 1f;

		public string showInvalidFieldName = "Show Invalid";
		public string showConditionsFieldName = "Show Conditions";

		public override void Awake()
		{
			base.Awake();
			HideAdditionalUI();
		}

		public override void ShowResponses(Subtitle subtitle, Response[] responses, float timeout)
		{
			responses = CheckInvalidResponses(responses);
			base.ShowResponses(subtitle, responses, timeout);
		}

		private Response[] CheckInvalidResponses(Response[] responses)
		{
			if (!HasAnyInvalid(responses)) return responses;

			var list = new List<Response>();
			
			for (int i = 0; i < responses.Length; i++)
			{
				var response = responses[i];
				// Is Show Invalid true?
				if (response.enabled || Field.LookupBool(response.destinationEntry.fields, showInvalidFieldName))
				{
					// Show Invalid is true, so make sure "Show Conditions" are true:
					var conditions = Field.LookupValue(response.destinationEntry.fields, showConditionsFieldName);

					if (Lua.IsTrue(conditions))
					{
						list.Add(response);
					}
				}
			}

			return list.ToArray();
		}

		private bool HasAnyInvalid(Response[] responses)
		{
			if (responses == null) return false;

			for (int i = 0; i < responses.Length; i++)
			{
				if (!responses[i].enabled)
					return true;
			}

			return false;
		}

		public override void HideSubtitle(Subtitle subtitle)
		{
			if (_previousSubtitle == subtitle)
				return;

			_previousSubtitle = subtitle;
			conversationUIElements.standardSubtitleControls.HideSubtitle(subtitle);
			AddPhraseToHistory(subtitle);
		}

		private void AddPhraseToHistory(Subtitle subtitle)
		{
			if (subtitle.formattedText.text.Length == 0)
				return;

			GameObject tempPhrase = Instantiate(_phraseExample, _conversationDisplay.transform);
			tempPhrase.GetComponentInChildren<TMP_Text>().text = subtitle.speakerInfo.Name;
			tempPhrase.GetComponentsInChildren<TMP_Text>()[1].text = subtitle.formattedText.text;
			_phrases.Add(tempPhrase);
			Canvas.ForceUpdateCanvases();

			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_conversationDisplay.transform);
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_scrollRect.transform);

			if (_scrollRect.verticalNormalizedPosition == 0f)
			{
				_scrollRect.verticalNormalizedPosition = Mathf.Abs(_delta - _scrollBar.size);
			}

			ScrollToBottom();
		}

		public void ClearHistory()
		{
			foreach (var item in _phrases)
			{
				Destroy(item);
			}

			_delta = 1f;
		}

		public void ShowAdditionalUI()
		{
			foreach (GameObject element in _additionalUI)
			{
				element.SetActive(true);
			}
		}

		public void HideAdditionalUI()
		{
			foreach (GameObject element in _additionalUI)
			{
				element.SetActive(false);
			}
		}

		private void ScrollToBottom()
		{
			if (_scrollCoroutine != null)
				StopCoroutine(_scrollCoroutine);

			_scrollCoroutine = StartCoroutine(ScrollToBottomCoroutine(_scrollRect));
		}

		private IEnumerator ScrollToBottomCoroutine(ScrollRect sr)
		{
			while (sr.verticalNormalizedPosition > 0f)
			{
				sr.verticalNormalizedPosition -= Time.deltaTime * _scrollSpeed;

				yield return null;
			}

			_delta = _scrollBar.size;
			sr.verticalNormalizedPosition = 0f;

			yield break;
		}
	}
}
