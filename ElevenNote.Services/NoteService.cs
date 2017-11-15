using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public IEnumerable<NoteListItemModel> GetNotes()
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                return
                    ctx
                        .Notes
                        .Where(e => e.OwnerID == _userId)
                        .Select(
                            e => 
                                new NoteListItemModel
                                {
                                    NoteID = e.NoteID,
                                    Title = e.Title,
                                    CreatedUtc = e.CreatedUtc,
                                    ModifiedUtc = e.ModifiedUtc
                                })      
                        .ToArray();
            }
        }

        public bool CreateNote(NoteCreateModel model)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity =
                    new NoteEntity
                    {
                        OwnerID = _userId,
                        Title = model.Title,
                        Content = model.Content,
                        CreatedUtc = DateTime.UtcNow
                    };

                ctx.Notes.Add(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
