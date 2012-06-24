﻿using FluentMigrator;

namespace Nebula.Migrations
{
    [Migration(4)]
    public class M0004_Add_System_Log : Migration
    {
        public const string SchemaName = "System";
        public const string TableName = "Log";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("HostName").AsAnsiString(255).NotNullable()
                .WithColumn("Level").AsAnsiString(50).NotNullable()
                .WithColumn("Logger").AsAnsiString(255).NotNullable()
                .WithColumn("Message").AsAnsiString(2000).NotNullable()
                .WithColumn("Exception").AsAnsiString(4000).Nullable()
                .WithColumn("SessionId").AsAnsiString(255).NotNullable()
                .WithColumn("UserName").AsAnsiString(255).NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}