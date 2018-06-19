namespace DbModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_weixin_list1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.weixin_list", "user_count", c => c.Int());
            DropColumn("dbo.weixin_list", "test");
        }
        
        public override void Down()
        {
            AddColumn("dbo.weixin_list", "test", c => c.String());
            AlterColumn("dbo.weixin_list", "user_count", c => c.Int(nullable: false));
        }
    }
}
