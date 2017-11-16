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

        public NoteDetailModel GetNoteById(int id)
        {
            NoteEntity entity;

            using (var ctx = new ElevenNoteDbContext())
            {
                entity =
                    ctx
                        .Notes
                        .SingleOrDefault(e => e.NoteID == id && e.OwnerID == _userId);
            }

            if (entity == null) return new NoteDetailModel();

            return
                new NoteDetailModel
                {
                    NoteID = entity.NoteID,
                    Title = entity.Title,
                    Content = entity.Content,
                    CreatedUtc = entity.CreatedUtc,
                    ModifiedUtc = entity.ModifiedUtc
                };
        }
    }
}
