using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using auctionapp.Models;
using Oracle.EntityFrameworkCore.Internal;

var builder = WebApplication.CreateBuilder(args);

// �������ݿ������ַ���
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ע�� AuctionDbContext��ʹ�� Oracle ���ݿ�
builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseOracle(connectionString,
    oracleOptions => oracleOptions.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion21)));

// ��������������������Swagger�ȣ�
builder.Services.AddControllers();

// ���ʹ�� Swagger �ĵ�
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ���� HTTP ����ܵ����м����
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auction API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
