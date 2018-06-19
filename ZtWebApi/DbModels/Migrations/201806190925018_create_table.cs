namespace DbModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.weixin_list",
                c => new
                    {
                        weixin_id = c.Int(nullable: false, identity: true),
                        weixin_name = c.String(nullable: false, maxLength: 255),
                        email = c.String(nullable: false),
                        password = c.String(maxLength: 255),
                        appid = c.String(nullable: false, maxLength: 255),
                        appsecret = c.String(nullable: false, maxLength: 255),
                        access_token = c.String(maxLength: 255),
                        access_token_time = c.DateTime(),
                        jsapi_ticket = c.String(),
                        jsapi_ticket_time = c.DateTime(),
                        pay_mch_id = c.String(),
                        pay_partner_key = c.String(),
                        mark = c.String(),
                        user_count = c.Int(nullable: false),
                        create_time = c.DateTime(),
                        update_time = c.DateTime(),
                    })
                .PrimaryKey(t => t.weixin_id);
            
            CreateTable(
                "dbo.weixin_menus",
                c => new
                    {
                        weixin_menus_id = c.Int(nullable: false, identity: true),
                        weixin_id = c.Int(nullable: false),
                        weixin_menu_parent_id = c.Int(nullable: false),
                        weixin_menu_name = c.String(nullable: false, maxLength: 255),
                        weixin_menu_type = c.String(maxLength: 255),
                        weixin_menu_key = c.String(maxLength: 64),
                        weixin_menu_url = c.String(maxLength: 255),
                        weixin_menu_media_id = c.String(maxLength: 255),
                        weixin_applet_appid = c.String(maxLength: 255),
                        weixin_applet_pagepath = c.String(maxLength: 255),
                        topsize = c.Int(nullable: false),
                        create_time = c.DateTime(),
                        update_time = c.DateTime(),
                    })
                .PrimaryKey(t => t.weixin_menus_id)
                .ForeignKey("dbo.weixin_list", t => t.weixin_id, cascadeDelete: true)
                .Index(t => t.weixin_id);
            
            CreateTable(
                "dbo.weixin_template",
                c => new
                    {
                        weixin_template_id = c.Int(nullable: false, identity: true),
                        weixin_id = c.Int(),
                        template_code = c.String(),
                        tempid = c.String(),
                        create_time = c.DateTime(),
                        update_time = c.DateTime(),
                    })
                .PrimaryKey(t => t.weixin_template_id)
                .ForeignKey("dbo.weixin_list", t => t.weixin_id)
                .Index(t => t.weixin_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.weixin_template", "weixin_id", "dbo.weixin_list");
            DropForeignKey("dbo.weixin_menus", "weixin_id", "dbo.weixin_list");
            DropIndex("dbo.weixin_template", new[] { "weixin_id" });
            DropIndex("dbo.weixin_menus", new[] { "weixin_id" });
            DropTable("dbo.weixin_template");
            DropTable("dbo.weixin_menus");
            DropTable("dbo.weixin_list");
        }
    }
}
