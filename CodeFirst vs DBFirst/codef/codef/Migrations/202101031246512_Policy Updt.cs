namespace codef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PolicyUpdt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Policies", "Customer_Name", c => c.String());
            AddColumn("dbo.Policies", "Employee_Name", c => c.String());
            AddColumn("dbo.Policies", "Insurance_Type", c => c.String());
            AddColumn("dbo.Policies", "Policy_Start_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Policies", "Policy_Expiration_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Policies", "Anual_Fee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Policies", "Info_About", c => c.String());
            AddColumn("dbo.Policies", "Coverage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Policies", "name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Policies", "name", c => c.String());
            DropColumn("dbo.Policies", "Coverage");
            DropColumn("dbo.Policies", "Info_About");
            DropColumn("dbo.Policies", "Anual_Fee");
            DropColumn("dbo.Policies", "Policy_Expiration_Date");
            DropColumn("dbo.Policies", "Policy_Start_Date");
            DropColumn("dbo.Policies", "Insurance_Type");
            DropColumn("dbo.Policies", "Employee_Name");
            DropColumn("dbo.Policies", "Customer_Name");
        }
    }
}
