using RobotMonitor.Components;
using SimpleMqtt;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//------------------------------------------------------------------------------------------------------------------------
builder.Services.AddSingleton<ISqlBatteryRepository>(provider =>
    new SqlBatteryRepository("Server=aei-sql2.avans.nl,1443;Database=DB2242722;User Id=ITI2242722;Password=S8yeQ6W0; TrustServerCertificate=True; "));
builder.Services.AddSingleton<ISqlCommandoRepository>(provider =>
    new SqlCommandoRepository("Server=aei-sql2.avans.nl,1443;Database=DB2242722;User Id=ITI2242722;Password=S8yeQ6W0; TrustServerCertificate=True; "));
builder.Services.AddSingleton<ISqlInteractionMomentsRepository>(provider =>
    new SqlInteractionMomentsRepository("Server=aei-sql2.avans.nl,1443;Database=DB2242722;User Id=ITI2242722;Password=S8yeQ6W0; TrustServerCertificate=True; "));
builder.Services.AddSingleton<ISqlSensorRepository>(provider =>
    new SqlSensorRepository("Server=aei-sql2.avans.nl,1443;Database=DB2242722;User Id=ITI2242722;Password=S8yeQ6W0; TrustServerCertificate=True; "));


string clientId = "Robot-" + Guid.NewGuid().ToString();
var mqttClient = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ(clientId);

builder.Services.AddSingleton(mqttClient);

builder.Services.AddHostedService<MQTTBatteryMessageProcessing>();
builder.Services.AddHostedService<MQTTCommandMessageProcessing>();
builder.Services.AddHostedService<MQTTInteractionMomentsMessageProcessing>();
builder.Services.AddHostedService<MQTTSensorMessageProcessing>();
builder.Services.AddHostedService<MQTTMessageSending>();

builder.Services.AddMudServices();

//-------------------------------------------------------------------------------------------------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
