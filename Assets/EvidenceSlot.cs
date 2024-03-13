using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceSlot : MonoBehaviour
{
	[SerializeField] private Image _itemImage;

	public void SetData(EvidenceItem evidenceItem)
	{
		_itemImage.sprite = evidenceItem.EvidenceIcon;
		_itemImage.color = new Color(255, 255, 255, 1);
	}

	public void SetData(EvidenceReport evidenceReport)
	{
		_itemImage.sprite = evidenceReport.Sprite;
		_itemImage.color = new Color(255, 255, 255, 1);
	}

	public void ClearData()
	{
		_itemImage.color = new Color(255, 255, 255, 0);
	}
}
