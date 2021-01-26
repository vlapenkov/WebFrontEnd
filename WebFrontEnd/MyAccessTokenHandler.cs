using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebFrontEnd
{
    public class MyAccessTokenHandler : DelegatingHandler
    {
        IHttpContextAccessor _contextAccessor;

        public MyAccessTokenHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            // берем access_token из контекста
            var accessToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");

            request.SetBearerToken(accessToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }

}
