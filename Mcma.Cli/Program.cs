using Mcma.Cli;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .RunCommandLineApplicationAsync<McmaCli>(args);