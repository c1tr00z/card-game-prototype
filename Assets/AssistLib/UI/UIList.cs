﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutGroup))]
public class UIList : MonoBehaviour {

    [SerializeField] private UIListItem listItemSource;

    private List<UIListItem> _listItems;

    public void UpdateList<T>(IEnumerable<T> items) {
        if (_listItems != null) {
            foreach (var listItem in _listItems) {
                Destroy(listItem.gameObject);
            }
        }

        _listItems = new List<UIListItem>();

        foreach (var item in items) {
            CreateListItem(item);
        }
    }

    private UIListItem CreateListItem(object item) {
        var listItem = listItemSource.Clone();
        listItem.transform.SetParent(transform, false);
        listItem.transform.localScale = Vector3.one;

        var rectTransform = listItem.transform as RectTransform;

        listItem.UpdateItem(item);
        _listItems.Add(listItem);
        return listItem;
    }
}
