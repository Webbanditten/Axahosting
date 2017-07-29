namespace AxaHosting.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Databases", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.Databases", "DatabaseType", c => c.Int(nullable: false));
            AddColumn("dbo.Servers", "Username", c => c.String());
            AddColumn("dbo.Servers", "Password", c => c.String());
            CreateIndex("dbo.Databases", "ProductId");
            AddForeignKey("dbo.Databases", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Databases", "ProductId", "dbo.Products");
            DropIndex("dbo.Databases", new[] { "ProductId" });
            DropColumn("dbo.Servers", "Password");
            DropColumn("dbo.Servers", "Username");
            DropColumn("dbo.Databases", "DatabaseType");
            DropColumn("dbo.Databases", "ProductId");
        }
    }
}
