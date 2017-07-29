namespace AxaHosting.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class loadbalance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PreparedIps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternalIp = c.String(nullable: false),
                        InternalIp = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Servers", "InternalHostname");
            DropColumn("dbo.Servers", "ExternalHostname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servers", "ExternalHostname", c => c.String(nullable: false, maxLength: 64));
            AddColumn("dbo.Servers", "InternalHostname", c => c.String(nullable: false, maxLength: 64));
            DropTable("dbo.PreparedIps");
        }
    }
}
