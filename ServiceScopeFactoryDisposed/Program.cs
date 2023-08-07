using System.Threading.Channels;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using ServiceScopeFactoryDisposed;

var builder = WebApplication.CreateBuilder(args);

// Works when this is commented out.
// builder.Host.UseLamar();

var channel = Channel.CreateUnbounded<WorkItem>(new UnboundedChannelOptions()
    {SingleReader = true, SingleWriter = false});
builder.Services.AddSingleton(channel.Reader);
builder.Services.AddSingleton(channel.Writer);

builder.Services.AddTransient<TestService>();

builder.Services.AddHostedService<QueueHostedService>();

var app = builder.Build();

app.MapGet("/test", async ([FromServices] TestService test) =>
{
    await test.Handle();
});

app.Run();