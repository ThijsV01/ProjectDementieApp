using RobotMonitor.Components;
using SimpleMqtt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//------------------------------------------------------------------------------------------------------------------------
string clientId = "Robot-" + Guid.NewGuid().ToString();
var mqttClient = SimpleMqttClient.CreateSimpleMqttClientForHiveMQ(clientId);
builder.Services.AddSingleton(mqttClient);
builder.Services.AddHostedService<MQTTMessageProcessingService>();
builder.Services.AddScoped<ISqlBatteryRepository>(provider =>
    new SqlBatteryRepository("Server=aei-sql2.avans.nl,1443;Database=DB2242722;User Id=ITI2242722;Password=S8yeQ6W0;"));

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
