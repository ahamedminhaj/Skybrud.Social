using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Globalization;
using Skybrud.Social.Interfaces.Http;

namespace Skybrud.Social.Http {

    /// <summary>
    /// A wrapper class extending the functionality of <see cref="NameValueCollection"/>.
    /// </summary>
    public class SocialHttpQueryString : IHttpQueryString {

        #region Private fields

        private readonly NameValueCollection _nvc;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a reference to the internal <see cref="NameValueCollection"/>.
        /// </summary>
        public NameValueCollection NameValueCollection {
            get { return _nvc; }
        }

        /// <summary>
        /// Gets the number of key/value pairs contained in the internal <see cref="NameValueCollection"/> instance.
        /// </summary>
        public int Count {
            get { return _nvc.Count; }
        }

        /// <summary>
        /// Gets whether the internal <see cref="NameValueCollection"/> is empty.
        /// </summary>
        public bool IsEmpty {
            get { return _nvc.Count == 0; }
        }

        /// <summary>
        /// Gets an array of the keys of all items in the query string.
        /// </summary>
        public string[] Keys {
            get { return _nvc.AllKeys; }
        }

        /// <summary>
        /// Gets an array of all items in the query string.
        /// </summary>
        public KeyValuePair<string, string>[] Items {
            get { return (from string key in _nvc.Keys select new KeyValuePair<string, string>(key, _nvc[key])).ToArray(); }
        }

        /// <summary>
        /// Gets the value of the item with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the item to match.</param>
        /// <returns>Returns the <see cref="string"/> value of the item, or <code>NULL</code> if not found.</returns>
        public string this[string key] {
            get { return _nvc[key]; }
        }

        /// <summary>
        /// Gets whether this implementation of <see cref="IHttpQueryString"/> supports duplicate keys.
        /// </summary>
        public bool SupportsDuplicateKeys {
            get { return false; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance without any entries.
        /// </summary>
        public SocialHttpQueryString() {
            _nvc = new NameValueCollection();
        }

        /// <summary>
        /// Initializes a new instance based on the specified <code>query</code>.
        /// </summary>
        /// <param name="query">An instance of <see cref="NameValueCollection"/> that should make up the query string.</param>
        public SocialHttpQueryString(NameValueCollection query) {
            _nvc = query ?? new NameValueCollection();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an entry with the specified <code>key</code> and <code>value</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <param name="value">The value of the entry.</param>
        public void Add(string key, object value) {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");
            _nvc.Add(key, String.Format(CultureInfo.InvariantCulture, "{0}", value));
        }

        /// <summary>
        /// Sets the <code>value</code> of an entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <param name="value">The value of the entry.</param>
        public void Set(string key, object value) {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");
            _nvc[key] = String.Format(CultureInfo.InvariantCulture, "{0}", value);
        }

        /// <summary>
        /// Gets a string representation of the query string.
        /// </summary>
        /// <returns>Returns the query string as an URL encoded string.</returns>
        public override string ToString() {
            return SocialUtils.Misc.NameValueCollectionToQueryString(_nvc);
        }

        /// <summary>
        /// Return whether the query string contains an entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns <code>true</code> if the query string contains the specified <code>key</code>, otherwise
        /// <code>false</code>.</returns>
        public bool ContainsKey(string key) {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");
            return _nvc.Get(key) != null || _nvc.AllKeys.Contains(key);
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> value of the entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <see cref="System.String"/> value of the entry, or <code>null</code> if not found.</returns>
        public string GetString(string key) {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");
            return _nvc[key];
        }

        /// <summary>
        /// Gets the <see cref="System.Int32"/> value of the entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <see cref="System.Int32"/> value of the entry, or <code>0</code> if not found.</returns>
        public int GetInt32(string key) {
            return GetValue<int>(key);
        }

        /// <summary>
        /// Gets the <see cref="System.Int64"/> value of the entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <see cref="System.Int64"/> value of the entry, or <code>0</code> if not found.</returns>
        public long GetInt64(string key) {
            return GetValue<long>(key);
        }

        /// <summary>
        /// Gets the <see cref="System.Boolean"/> value of the entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <see cref="System.Boolean"/> value of the entry, or <code>0</code> if not found.</returns>
        public bool GetBoolean(string key) {
            return GetValue<bool>(key);
        }

        /// <summary>
        /// Gets the <see cref="System.Double"/> value of the entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <see cref="System.Double"/> value of the entry, or <code>0</code> if not found.</returns>
        public double GetDouble(string key) {
            return GetValue<double>(key);
        }

        /// <summary>
        /// Gets the <see cref="System.Single"/> value of the entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <see cref="System.Single"/> value of the entry, or <code>0</code> if not found.</returns>
        public float GetFloat(string key) {
            return GetValue<float>(key);
        }

        /// <summary>
        /// Gets the <code>T</code> value of the entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <code>T</code> value of the entry, or the default value of <code>T</code> if not found.</returns>
        private T GetValue<T>(string key) {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");
            string value = _nvc[key];
            return String.IsNullOrWhiteSpace(value) ? default(T) : (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        #endregion

        #region Operator overloading

        /// <summary>
        /// Initializes a new instance based on the specified <code>query</code>.
        /// </summary>
        /// <param name="query">An instance of <see cref="NameValueCollection"/> that should make up the query string.</param>
        /// <returns>An instance of <see cref="SocialHttpQueryString"/> based on the specified <code>query</code>.</returns>
        public static implicit operator SocialHttpQueryString(NameValueCollection query) {
            return query == null ? null : new SocialHttpQueryString(query);
        }

        #endregion

    }

}