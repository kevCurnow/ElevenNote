﻿using ElevenNote.Data;
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
                entity = GetNoteById(ctx, id);
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

        public bool UpdateNote(NoteEditModel model)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity = GetNoteById(ctx, model.NoteID);
                  
                if (entity == null) return false;

                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.ModifiedUtc = DateTime.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        private NoteEntity GetNoteById(ElevenNoteDbContext context, int noteID)
        {
            return
                context
                    .Notes
                    .SingleOrDefault(e => e.NoteID == noteID && e.OwnerID == _userId);
        }

        public bool DeleteNote(int noteID)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity = GetNoteById(ctx, noteID);

                if (entity == null) return false;

                ctx.Notes.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
