namespace OTProyect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatetablePuntosCalorOficial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.puntoscalor",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        idpuntocalor = c.Int(nullable: false),
                        country_id = c.String(),
                        latitude = c.Double(nullable: false),
                        longitude = c.Double(nullable: false),
                        bright_ti4 = c.Double(nullable: false),
                        track = c.Double(nullable: false),
                        acq_date = c.DateTime(nullable: false),
                        acq_time = c.Int(nullable: false),
                        satellite = c.String(),
                        instrument = c.String(),
                        confidence = c.String(),
                        version = c.String(),
                        bright_ti5 = c.Double(nullable: false),
                        frp = c.Double(nullable: false),
                        daynight = c.String(),
                        fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.puntoscalor");
        }
    }
}
