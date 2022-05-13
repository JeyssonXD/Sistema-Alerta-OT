namespace OTProyect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EliminarColumnaPuntoCalor : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.puntoscalor", "idpuntocalor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.puntoscalor", "idpuntocalor", c => c.Int(nullable: false));
        }
    }
}
