// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SiteOfRefuge.API.Models;

namespace SiteOfRefuge.API
{
    public class RefugeesApi
    {
        private ILogger<RefugeesApi> _logger;

        /// <summary> Initializes a new instance of RefugeesApi. </summary>
        /// <param name="logger"> Class logger. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="logger"/> is null. </exception>
        public RefugeesApi(ILogger<RefugeesApi> logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _logger = logger;
        }

        /// <summary> Get a summary list of refugees registered in the system. </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName("GetRefugeesAsync_get")]
        public async Task<IActionResult> GetRefugeesAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "refugees")] HttpRequest req, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("HTTP trigger function processed a request.");

            // TODO: Handle Documented Responses.
            // Spec Defines: HTTP 200

            IEnumerable<RefugeeSummary> refugeeList = new List<RefugeeSummary> {
                new RefugeeSummary( new Guid("3F2504E0-4F89-41D3-9A0C-0305E82C3301"), "PL-06", 1, new DateTimeOffset(DateTime.UtcNow) ),
                new RefugeeSummary( new Guid("2D2503E0-4D89-41C6-2D3E-1263EF2B1829"), "PL-16", 4, new DateTimeOffset(DateTime.UtcNow) )
            };
            
            return new OkObjectResult(refugeeList);
        }

        /// <summary> Registers a new refugee in the system. </summary>
        /// <param name="body"> The Refugee to use. </param>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="body"/> is null. </exception>
        [FunctionName("AddRefugeeAsync_post")]
        public async Task<IActionResult> AddRefugeeAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "refugees")] Refugee body, HttpRequest req, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("HTTP trigger function processed a request.");

            // TODO: Handle Documented Responses.
            // Spec Defines: HTTP 201

            throw new NotImplementedException();
        }

        /// <summary> Get information about a specific refugee. </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="id"> Refugee id in UUID/GUID format. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName("GetRefugeeAsync_get")]
        public async Task<IActionResult> GetRefugeeAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "refugees/{id}")] HttpRequest req, Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("HTTP trigger function processed a request.");

            // TODO: Handle Documented Responses.
            // Spec Defines: HTTP 200
            // Spec Defines: HTTP 404

            throw new NotImplementedException();
        }

        /// <summary> Updates a refugee in the system. </summary>
        /// <param name="id"> Refugee id in UUID/GUID format. </param>
        /// <param name="body"> The Refugee to use. </param>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="body"/> is null. </exception>
        [FunctionName("UpdateRefugeeAsync_put")]
        public async Task<IActionResult> UpdateRefugeeAsync(Guid id, [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "refugees/{id}")] Refugee body, HttpRequest req, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("HTTP trigger function processed a request.");

            // TODO: Handle Documented Responses.
            // Spec Defines: HTTP 204
            // Spec Defines: HTTP 404

            throw new NotImplementedException();
        }

        /// <summary> Schedules a refugee to be deleted from the system (after 7 days archival). </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="id"> Refugee id in UUID/GUID format. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName("DeleteRefugeeAsync_delete")]
        public async Task<IActionResult> DeleteRefugeeAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "refugees/{id}")] HttpRequest req, Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("HTTP trigger function processed a request.");

            // TODO: Handle Documented Responses.
            // Spec Defines: HTTP 202
            // Spec Defines: HTTP 404

            throw new NotImplementedException();
        }
    }
}
