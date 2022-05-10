namespace OTProyect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Createtablepuntoscalor : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PuntosCalorModels", newName: "puntoscalors");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.puntoscalors", newName: "PuntosCalorModels");
        }
    }
}
