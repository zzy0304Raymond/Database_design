using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using auctionapp.Models;
using Oracle.EntityFrameworkCore.Internal;

var builder = WebApplication.CreateBuilder(args);

var corsPolicyName = "_AllowOrigins";

// 配置数据库连接字符串
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 注入 AuctionDbContext，使用 Oracle 数据库
builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseOracle(connectionString,
    oracleOptions => oracleOptions.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion21)));

// 添加 CORS 服务
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy.AllowAnyOrigin() // 允许所有来源
              .AllowAnyHeader() // 允许所有请求头
              .AllowAnyMethod(); // 允许所有请求方法 (GET, POST, PUT, DELETE, etc.)
    });
});

// 添加其他服务（如控制器、Swagger等）
builder.Services.AddControllers();

// 如果使用 Swagger 文档
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 配置 HTTP 请求管道（中间件）
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auction API v1"));
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
