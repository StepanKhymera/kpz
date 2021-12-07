namespace codef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PolicyComplete : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Policies");
            CreateTable(
               "dbo.Policies",
               c => new
               {
                   PolicyID = c.Int(nullable: false, identity: true),
                   Customer_Name = c.String(),
                   Employee_Name = c.String(),
                   Insurance_Type = c.String(),
                   Policy_Start_Date = c.DateTime(),
                   Policy_Expiration_Date = c.DateTime(),
                   Anual_Fee = c.Decimal(),
                   Info_About = c.String(),
                   Coverage = c.Decimal()
               })
               .PrimaryKey(t => t.PolicyID);
        }
        
        public override void Down()
        {
            DropTable("dbo.Policies");
        }
    }
}
