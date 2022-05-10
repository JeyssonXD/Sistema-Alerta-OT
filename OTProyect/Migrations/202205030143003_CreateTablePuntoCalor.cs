namespace OTProyect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTablePuntoCalor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PuntosCalorModels",
                c => new
                    {
                        idpuntocalor = c.Int(nullable: false, identity: true),
                        geom = c.String(),
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
                    })
                .PrimaryKey(t => t.idpuntocalor);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PuntosCalorModels");
        }
    }
}
