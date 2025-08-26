using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MU.CV.DAL.Entities.Note;

namespace MU.CV.DAL.Db.Configurations;

internal class NotesDbTableConfiguration : IEntityTypeConfiguration<NoteDAL>
{
    public void Configure(EntityTypeBuilder<NoteDAL> builder)
    {
        builder
            .ToTable("Notes")
            .HasKey(x => x.Id);
    }
}