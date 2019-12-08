﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageWithIndependentRoundedCorners : MonoBehaviour {
	[SerializeField] private Vector4 r;
	[SerializeField] private Vector2 size;
	[SerializeField] private Material material;
	
	// xy - position,
	// zw - halfSize
	[HideInInspector, SerializeField] private Vector4 rect2props;

	private readonly int prop_halfSize = Shader.PropertyToID("_halfSize");
	private readonly int prop_radiuses = Shader.PropertyToID("_r");
	private readonly int prop_rect2props = Shader.PropertyToID("_rect2props");
	protected void OnValidate(){
		Eval();
	}
	
	private void OnRenderObject(){
		size = ((RectTransform) transform).rect.size;
		Eval();
	}

	// Vector2.right rotated clockwise by 45 degrees
	private static readonly Vector2 wNorm = new Vector2(.7071068f, -.7071068f);
	// Vector2.right rotated counter-clockwise by 45 degrees
	private static readonly Vector2 hNorm = new Vector2(.7071068f, .7071068f);
	
	private void Eval(){

		// Vector that goes from left to right sides of rect2
		var aVec = new Vector2(size.x, -size.y + r.x + r.z);

		// Project vector aVec to wNorm to get magnitude of rect2 width vector
		var halfWidth = Vector2.Dot(aVec, wNorm) * .5f;
		rect2props.z = halfWidth;
		
		
		// Vector that goes from bottom to top sides of rect2
		var bVec = new Vector2(size.x, size.y - r.w - r.y);
		
		// Project vector bVec to hNorm to get magnitude of rect2 height vector
		var halfHeight = Vector2.Dot(bVec, hNorm) * .5f;
		rect2props.w = halfHeight;
		
		
		// Vector that goes from left to top sides of rect2
		var efVec = new Vector2(size.x - r.x - r.y, 0);
		// Vector that goes from point E to point G, which is top-left of rect2
		var egVec = hNorm * Vector2.Dot(efVec, hNorm);
		// Position of point E relative to center of coord system
		var ePoint = new Vector2(r.x - (size.x / 2), size.y / 2);
		// Origin of rect2 relative to center of coord system
		// ePoint + egVec == vector to top-left corner of rect2
		// wNorm * halfWidth + hNorm * -halfHeight == vector from top-left corner to center
		var origin = ePoint + egVec + wNorm * halfWidth + hNorm * -halfHeight;
		rect2props.x = origin.x;
		rect2props.y = origin.y;
		//
		material.SetVector(prop_rect2props, rect2props);
		material.SetVector(prop_halfSize, size * .5f);
		material.SetVector(prop_radiuses, r);
	}
    
}
