//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.Identity.Core.Cache
{
    /// <summary>
    /// Determines what type of subject the token was issued for.
    /// </summary>
    internal enum TokenSubjectType
    {
        /// <summary>
        /// User
        /// </summary>
        User,
        /// <summary>
        /// Client
        /// </summary>
        Client,
        /// <summary>
        /// UserPlusClient: This is for confidential clients used in middle tier.
        /// </summary>
        UserPlusClient
    };

    /// <summary>
    /// <see cref="AdalTokenCacheKey"/> can be used with Linq to access items from the TokenCache dictionary.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal sealed class AdalTokenCacheKey
    {
        internal AdalTokenCacheKey(string authority, string resource, string clientId, TokenSubjectType tokenSubjectType, AdalUserInfo adalUserInfo)
            : this(authority, resource, clientId, tokenSubjectType, adalUserInfo?.UniqueId, adalUserInfo?.DisplayableId)
        {
        }

        internal AdalTokenCacheKey(string authority, string resource, string clientId, TokenSubjectType tokenSubjectType, string uniqueId, string displayableId)
        {
            Authority = authority;
            Resource = resource;
            ClientId = clientId;
            TokenSubjectType = tokenSubjectType;
            UniqueId = uniqueId;
            DisplayableId = displayableId;
        }

        public string Authority { get; }

        public string Resource { get; }

        public string ClientId { get; }

        public string UniqueId { get; }

        public string DisplayableId { get; }

        public TokenSubjectType TokenSubjectType { get; }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return (obj is AdalTokenCacheKey other) && Equals(other);
        }

        /// <summary>
        /// Determines whether the specified TokenCacheKey is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified TokenCacheKey is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="other">The TokenCacheKey to compare with the current object. </param><filterpriority>2</filterpriority>
        public bool Equals(AdalTokenCacheKey other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other != null
                && other.Authority == Authority
                && ResourceEquals(other.Resource)
                && ClientIdEquals(other.ClientId)
                && other.UniqueId == UniqueId
                && DisplayableIdEquals(other.DisplayableId)
                && other.TokenSubjectType == TokenSubjectType;
        }

        /// <summary>
        /// Returns the hash code for this TokenCacheKey.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode()
        {
            const string delimiter = ":::";
            var hashString = Authority + delimiter
                           + Resource.ToLowerInvariant() + delimiter
                           + ClientId.ToLowerInvariant() + delimiter
                           + UniqueId + delimiter
                           + DisplayableId?.ToLowerInvariant() + delimiter
                           + (int) TokenSubjectType;
            return hashString.GetHashCode();
        }

        internal bool ResourceEquals(string otherResource)
        {
            return string.Compare(otherResource, Resource, StringComparison.OrdinalIgnoreCase) == 0;
        }

        internal bool ClientIdEquals(string otherClientId)
        {
            return string.Compare(otherClientId, ClientId, StringComparison.OrdinalIgnoreCase) == 0;
        }

        internal bool DisplayableIdEquals(string otherDisplayableId)
        {
            return string.Compare(otherDisplayableId, DisplayableId, StringComparison.OrdinalIgnoreCase) == 0;
        }

        private string DebuggerDisplay
        {
            get { return string.Format(
                CultureInfo.InvariantCulture, 
                "AdalTokenCacheKey: {0} {1} {2} {3} {4}", 
                Authority, Resource, ClientId, UniqueId, DisplayableId); }
        }
    }
}
