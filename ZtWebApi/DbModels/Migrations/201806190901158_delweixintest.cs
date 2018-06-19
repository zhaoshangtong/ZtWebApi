namespace DbModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delweixintest : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.weixin_list", "test");
        }
        
        public override void Down()
        {
            AddColumn("dbo.weixin_list", "test", c => c.String());
        }
    }
}
