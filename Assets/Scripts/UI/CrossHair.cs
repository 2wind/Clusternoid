using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrossHair : MonoBehaviour 
{
	[SerializeField]
	private Transform[] innerPartsT;

	[SerializeField]
	private Transform[] outerPartsT;

	[SerializeField]
	private Transform innerBaseT;

	[SerializeField]
	private Transform outerBaseT;

	private float innerFasten = -3;

	private float outerFasten = -5;

	private float normalSpeed = 30f;

	private float shootingSpeed = 90f;

	private float apperture;
	private float Apperture
	{
		get
		{
			return apperture;
		}
		set
		{
			apperture = value;
			foreach (var trans in innerPartsT)
			{
				trans.localPosition = Vector3.up * innerFasten * (1 - value);
			}
			foreach (var trans in outerPartsT)
			{
				trans.localPosition = Vector3.up * outerFasten * (1 - value);
			}
		}
	}

	Vector3 screenCenter;

	private void SetApperture()
	{
		foreach (var trans in innerPartsT)
		{
			trans.localPosition = Vector3.up * innerFasten * (1 - Apperture);
		}
		foreach (var trans in outerPartsT)
		{
			trans.localPosition = Vector3.up * outerFasten * (1 - Apperture);
		}
	}

	private void OnShoot()
	{
		DOTween.To(() => Apperture, (x) => Apperture = x, 1f, 0.15f).SetEase(Ease.OutQuad).SetRecyclable(true).
				OnComplete(() => DOTween.To(() => Apperture, (x) => Apperture = x, 0f, 0.15f).SetEase(Ease.InQuad).SetRecyclable(true));
	}

	private void Start()
	{
		Cursor.visible = false;
		Apperture = 0f;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			OnShoot();
		}

		if (Input.GetMouseButton(0))
		{
			innerBaseT.transform.localEulerAngles += Vector3.forward * shootingSpeed * Time.deltaTime * 0.5f;
			outerBaseT.transform.localEulerAngles -= Vector3.forward * shootingSpeed * Time.deltaTime;
		}
		else
		{
			innerBaseT.transform.localEulerAngles += Vector3.forward * normalSpeed * Time.deltaTime * 0.5f;
			outerBaseT.transform.localEulerAngles -= Vector3.forward * normalSpeed * Time.deltaTime;
		}

		transform.position = Input.mousePosition;
	}
}
