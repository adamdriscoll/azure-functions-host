﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.WebJobs.Script.Abstractions;
using Microsoft.Azure.WebJobs.Script.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Azure.WebJobs.Script.Rpc
{
    internal class NodeWorkerProvider : IWorkerProvider
    {
        public WorkerDescription GetDescription() => new WorkerDescription
        {
            Language = "Node",
            Extension = ".js",
            DefaultExecutablePath = "node",
            DefaultWorkerPath = Path.Combine("dist", "src", "nodejsWorker.js"),
        };

        public bool TryConfigureArguments(ArgumentsDescription args, IConfiguration config, ILogger logger)
        {
            var options = new DefaultWorkerOptions();
            config.GetSection("workers:node").Bind(options);
            if (options.TryGetDebugPort(out int debugPort))
            {
                args.ExecutableArguments.Add($"--inspect={debugPort}");
            }
            return true;
        }
    }
}
