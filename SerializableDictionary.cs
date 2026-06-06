using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities {

	/// <summary>
	/// A serializable key-value pair used to back SerializableDictionary.
	/// Unity's serializer can handle lists of these structs, which lets us
	/// persist dictionary data through serialization rounds.
	/// </summary>
	[Serializable]
	public struct SerializableKeyValuePair<TKey, TValue> {
		public TKey Key;
		public TValue Value;

		public SerializableKeyValuePair(TKey key, TValue value) {
			Key = key;
			Value = value;
		}
	}

	/// <summary>
	/// A dictionary that Unity's serializer can actually handle. It's backed by
	/// a List of SerializableKeyValuePair structs and implements
	/// ISerializationCallbackReceiver to sync that list with a real Dictionary
	/// at runtime.
	/// 
	/// Usage: inherit from this with concrete types so Unity can serialize it,
	/// e.g. `[Serializable] public class MyLookup : SerializableDictionary<string, int> {}`.
	/// </summary>
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver {

		[SerializeField]
		private List<SerializableKeyValuePair<TKey, TValue>> entries = new List<SerializableKeyValuePair<TKey, TValue>>();

		private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

		// ---------------------------------------------------------------
		// ISerializationCallbackReceiver
		// ---------------------------------------------------------------

		public void OnBeforeSerialize() {
			// We only convert from dictionary -> list here when we want to
			// persist changes made at runtime. For now we let the list drive
			// deserialization, so this is intentionally a no-op unless you
			// want to write back. Override in subclasses if needed.
		}

		public void OnAfterDeserialize() {
			dictionary.Clear();

			foreach (var entry in entries) {
				if (!ContainsKey(entry.Key)) {
					dictionary[entry.Key] = entry.Value;
				}
			}
		}

		// ---------------------------------------------------------------
		// IDictionary<TKey, TValue>
		// ---------------------------------------------------------------

		public TValue this[TKey key] {
			get => dictionary[key];
			set {
				dictionary[key] = value;
				UpsertEntry(key, value);
			}
		}

		public ICollection<TKey> Keys => dictionary.Keys;
		public ICollection<TValue> Values => dictionary.Values;
		public int Count => dictionary.Count;
		public bool IsReadOnly => false;

		public void Add(TKey key, TValue value) {
			dictionary.Add(key, value);
			entries.Add(new SerializableKeyValuePair<TKey, TValue>(key, value));
		}

		public void Add(KeyValuePair<TKey, TValue> item) {
			Add(item.Key, item.Value);
		}

		public bool Remove(TKey key) {
			if (!dictionary.Remove(key)) {
				return false;
			}

			entries.RemoveAll(e => EqualityComparer<TKey>.Default.Equals(e.Key, key));
			return true;
		}

		public bool Remove(KeyValuePair<TKey, TValue> item) {
			return Remove(item.Key);
		}

		public void Clear() {
			dictionary.Clear();
			entries.Clear();
		}

		public bool ContainsKey(TKey key) {
			return dictionary.ContainsKey(key);
		}

		public bool Contains(KeyValuePair<TKey, TValue> item) {
			return dictionary.TryGetValue(item.Key, out var value)
				&& EqualityComparer<TValue>.Default.Equals(value, item.Value);
		}

		public bool TryGetValue(TKey key, out TValue value) {
			return dictionary.TryGetValue(key, out value);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
			((ICollection<KeyValuePair<TKey, TValue>>)dictionary).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
			return dictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		// ---------------------------------------------------------------
		// Helpers
		// ---------------------------------------------------------------

		private void UpsertEntry(TKey key, TValue value) {
			for (int i = 0; i < entries.Count; i++) {
				if (EqualityComparer<TKey>.Default.Equals(entries[i].Key, key)) {
					entries[i] = new SerializableKeyValuePair<TKey, TValue>(key, value);
					return;
				}
			}

			entries.Add(new SerializableKeyValuePair<TKey, TValue>(key, value));
		}
	}

}