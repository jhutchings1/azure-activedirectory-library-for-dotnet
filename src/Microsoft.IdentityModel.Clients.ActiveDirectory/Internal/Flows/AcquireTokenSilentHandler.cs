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

using System;
using System.Threading.Tasks;
using Microsoft.Identity.Core;
using Microsoft.Identity.Core.Cache;

namespace Microsoft.IdentityModel.Clients.ActiveDirectory.Internal.Flows
{
    internal class AcquireTokenSilentHandler : AcquireTokenHandlerBase
    {
        public AcquireTokenSilentHandler(
            IServiceBundle serviceBundle, 
            RequestData requestData, 
            UserIdentifier userId, 
            IPlatformParameters parameters)
            : base(serviceBundle, requestData)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId", AdalErrorMessage.SpecifyAnyUser);
            }
            requestData.SubjectType = requestData.ClientKey.HasCredential
                ? TokenSubjectType.UserPlusClient
                : TokenSubjectType.User;
            UniqueId = userId.UniqueId;
            DisplayableId = userId.DisplayableId;
            UserIdentifierType = userId.Type;
            BrokerHelper.PlatformParameters = parameters;    
            SupportADFS = true;

            BrokerParameters[BrokerParameter.Username] = userId.Id;
            BrokerParameters[BrokerParameter.UsernameType] = userId.Type.ToString();
            BrokerParameters[BrokerParameter.SilentBrokerFlow] = null; //add key
        }

        protected internal /* internal for test only */ override Task<AdalResultWrapper> SendTokenRequestAsync()
        {
            if (ResultEx == null)
            {
                RequestContext.Logger.Verbose("No token matching arguments found in the cache");

                throw new AdalSilentTokenAcquisitionException();
            }

            throw new AdalSilentTokenAcquisitionException(ResultEx.Exception);
        }

        protected override void AddAdditionalRequestParameters(DictionaryRequestParameters requestParameters)
        {            
        }
    }
}
