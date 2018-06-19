namespace DbModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_weixin_list : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.weixin_list", "test", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.weixin_list", "test");
        }
    }
}
