using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BrainWave.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddStorage(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(async options =>
    {
        string validAudience = "api1";
        string validIssuer = "https://localhost:5001";
        string authority = "https://localhost:5001";
        
        options.Authority = authority;
        options.Audience = validAudience; 

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = validAudience,
            ValidIssuer = validIssuer,
            ValidateLifetime = true,
            
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = await GetSigningKeys()
        };


        async Task<IEnumerable<SecurityKey>> GetSigningKeys()
        {
            HttpClient httpClient = new HttpClient();
            var metadataRequest =
                new HttpRequestMessage(HttpMethod.Get, $"{authority}/.well-known/openid-configuration");
            var metadataResponse = await httpClient.SendAsync(metadataRequest);

            string content = await metadataResponse.Content.ReadAsStringAsync();
            var payload = JObject.Parse(content);
            string jwksUri = payload.Value<string>("jwks_uri");

            var keysRequest = new HttpRequestMessage(HttpMethod.Get, jwksUri);
            var keysResponse = await httpClient.SendAsync(keysRequest);
            var keysPayload = await keysResponse.Content.ReadAsStringAsync();
            var signingKeys = new JsonWebKeySet(keysPayload).Keys;

            return signingKeys;
        }
    })
    .AddOAuth2Introspection("introspection", options =>
    {
        options.Authority = "https://localhost:5001";
        options.ClientId = "api1";
        options.ClientSecret = "api1_secret";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
        policy.RequireClaim("scope", "profile");
    });
});
// Add services to the container.builder.Services.AddStorage(builder.Configuration);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(options =>
{
    options.AllowAnyOrigin()
    .AllowAnyHeader() 
    .AllowAnyMethod();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
