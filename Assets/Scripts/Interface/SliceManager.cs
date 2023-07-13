﻿using System.Collections.Generic;
using Maps;
using Objects;
using UnityEngine;
using Utility;

namespace Interface {
	public class SliceManager : MonoBehaviour {
		public GameObject templateSlice;
		public GameObject templateProjection;
		private readonly Dictionary<Source, List<ArraySlice>> slices = new();
		private readonly Dictionary<ArraySlice, Projection> projections = new();

		public void Slice(Source source, Map map) {
			var instance2 = Instantiate(templateProjection, transform);
			var image = instance2.GetComponent<Projection>();
			
			if (!slices.ContainsKey(source)) {
				slices[source] = new List<ArraySlice>();
			}
			var instance = Instantiate(templateSlice, Vector3.zero, Quaternion.identity);
			var slice = instance.GetComponent<ArraySlice>();
			slices[source].Add(slice);
			projections.Add(slice, image);
			image.Update += slice.Update;
			slice.Draw += image.Project;
			
			var colorGrid = Enumeration.ToArray(map.Grid.Cells, map.Colors, new Color32(0, 0, 0, 0));
			slice.Initialize(colorGrid, map.Grid.Size, map.Grid.Boundaries);
		}
		public void Remove(Source source) {
			foreach (var slice in slices[source]) {
				Destroy(projections[slice].gameObject);
				Destroy(slice.gameObject);
				projections.Remove(slice);
			}
			slices.Remove(source);
		}
	}
}