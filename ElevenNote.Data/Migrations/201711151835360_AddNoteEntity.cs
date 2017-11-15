namespace ElevenNote.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNoteEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Note",
                c => new
                    {
                        NoteID = c.Int(nullable: false, identity: true),
                        OwnerID = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Content = c.String(maxLength: 500),
                        CreatedUtc = c.DateTime(nullable: false),
                        ModifiedUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.NoteID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Note");
        }
    }
}
