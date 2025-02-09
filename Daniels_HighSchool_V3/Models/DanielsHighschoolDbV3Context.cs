using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Daniels_HighSchool_V3.Models;

public partial class DanielsHighschoolDbV3Context : DbContext
{
    public DanielsHighschoolDbV3Context()
    {
    }

    public DanielsHighschoolDbV3Context(DbContextOptions<DanielsHighschoolDbV3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassSubject> ClassSubjects { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<StaffOverview> Staff { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = localhost; Database = Daniels_Highschool_DB_V3; Integrated Security = True; Trust Server Certificate = True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Classes__CB1927C0AF742C91");

            entity.Property(e => e.ClassName).HasMaxLength(50);

            entity.HasOne(d => d.Mentor).WithMany(p => p.Classes)
                .HasForeignKey(d => d.MentorId)
                .HasConstraintName("FK__Classes__MentorI__3F466844");
        });

        modelBuilder.Entity<ClassSubject>(entity =>
        {
            entity.HasKey(e => new { e.ClassId, e.SubjectId, e.Semester }).HasName("PK__Class_Su__765E3852AED7ABE9");

            entity.ToTable("Class_Subject");

            entity.Property(e => e.Semester)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Class).WithMany(p => p.ClassSubjects)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Class_Sub__Class__59063A47");

            entity.HasOne(d => d.Subject).WithMany(p => p.ClassSubjects)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Class_Sub__Subje__59FA5E80");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BED5FA125B7");

            entity.Property(e => e.DepartmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__Grades__54F87A57123709C3");

            entity.Property(e => e.Grade1)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Grade");
            entity.Property(e => e.GradeDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Staff).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Grades__StaffId__52593CB8");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Grades__StudentI__5070F446");

            entity.HasOne(d => d.Subject).WithMany(p => p.Grades)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Grades__SubjectI__5165187F");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK__Position__60BB9A79B68BBED2");

            entity.Property(e => e.PositionName).HasMaxLength(100);

            entity.HasOne(d => d.Department).WithMany(p => p.Positions)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Positions__Depar__398D8EEE");
        });

        modelBuilder.Entity<StaffOverview>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AB1718A862E6");

            entity.HasIndex(e => e.PersonalNumber, "UQ__Staff__AC2CC42EA674DB13").IsUnique();

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MonthlySalary).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PersonalNumber)
                .HasMaxLength(13)
                .IsUnicode(false);

            entity.HasOne(d => d.Position).WithMany(p => p.Staff)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Staff__PositionI__3C69FB99");

            entity.HasMany(d => d.Subjects).WithMany(p => p.Staff)
                .UsingEntity<Dictionary<string, object>>(
                    "StaffSubject",
                    r => r.HasOne<Subject>().WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Staff_Sub__Subje__47DBAE45"),
                    l => l.HasOne<StaffOverview>().WithMany()
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Staff_Sub__Staff__46E78A0C"),
                    j =>
                    {
                        j.HasKey("StaffId", "SubjectId").HasName("PK__Staff_Su__0C15112D77F34E5C");
                        j.ToTable("Staff_Subject");
                    });
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B990F085BC3");

            entity.HasIndex(e => e.PersonalNumber, "UQ__Students__AC2CC42E72310D04").IsUnique();

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PersonalNumber)
                .HasMaxLength(13)
                .IsUnicode(false);

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__ClassI__4222D4EF");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subjects__AC1BA3A89C0F4C6C");

            entity.Property(e => e.SubjectName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
