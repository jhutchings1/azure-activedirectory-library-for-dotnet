﻿//----------------------------------------------------------------------
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

using Microsoft.IdentityModel.Clients.ActiveDirectory.Internal.OAuth2;
using System;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Clients.ActiveDirectory.Internal.Platform
{
    [Android.Runtime.Preserve(AllMembers = true)]
    internal class PlatformInformation : PlatformInformationBase
    {
        public override void AddPromptBehaviorQueryParameter(IPlatformParameters parameters, DictionaryRequestParameters authorizationRequestParameters)
        {
            if (!(parameters is PlatformParameters authorizationParameters))
            {
                throw new ArgumentException("parameters should be of type PlatformParameters", nameof(parameters));
            }

            PromptBehavior promptBehavior = authorizationParameters.PromptBehavior;

            switch (promptBehavior)
            {
                case PromptBehavior.Always:
                    authorizationRequestParameters[OAuthParameter.Prompt] = PromptValue.Login;
                    break;
                case PromptBehavior.SelectAccount:
                    authorizationRequestParameters[OAuthParameter.Prompt] = PromptValue.SelectAccount;
                    break;
                case PromptBehavior.RefreshSession:
                    authorizationRequestParameters[OAuthParameter.Prompt] = PromptValue.RefreshSession;
                    break;
            }
        }

        public override bool GetCacheLoadPolicy(IPlatformParameters parameters)
        {
            if (!(parameters is PlatformParameters authorizationParameters))
            {
                throw new ArgumentException("parameters should be of type PlatformParameters", nameof(parameters));
            }

            PromptBehavior promptBehavior = authorizationParameters.PromptBehavior;

            return promptBehavior != PromptBehavior.Always && promptBehavior != PromptBehavior.RefreshSession &&
                   promptBehavior != PromptBehavior.SelectAccount;
        }
    }
}
