using Microsoft.EntityFrameworkCore;

namespace DataProcessor.Repositories.SqlServer.Models
{
    public partial class DataProcessorContext : DbContext
    {
        public DataProcessorContext()
        {
        }

        public DataProcessorContext(DbContextOptions<DataProcessorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<FileStatus> FileStatus { get; set; }
        public virtual DbSet<FileType> FileType { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<ValidationResult> ValidationResult { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>(entity =>
            {
                entity.Property(e => e.FileId).HasColumnName("FileID");

                entity.Property(e => e.CustomData)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.FileStatusId).HasColumnName("FileStatusID");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.ValidationResultId).HasColumnName("ValidationResultID");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FileCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.FileLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FileStatus>(entity =>
            {
                entity.Property(e => e.FileStatusId)
                    .HasColumnName("FileStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FileStatusDescription).HasMaxLength(255);

                entity.Property(e => e.FileStatusName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FileType>(entity =>
            {
                entity.Property(e => e.FileTypeId)
                    .HasColumnName("FileTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FileTypeDescription).HasMaxLength(255);

                entity.Property(e => e.FileTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName)
                    .HasName("IX_User")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedNever();

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ValidationResult>(entity =>
            {
                entity.Property(e => e.ValidationResultId)
                    .HasColumnName("ValidationResultID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ValidationResultDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ValidationResultName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
