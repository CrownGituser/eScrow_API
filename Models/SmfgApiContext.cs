using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SMFG_API_New.Models;

public partial class SmfgApiContext : DbContext
{
    public SmfgApiContext()
    {
    }

    public SmfgApiContext(DbContextOptions<SmfgApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<InwardDump> InwardDumps { get; set; }

    public virtual DbSet<InwardFile> InwardFiles { get; set; }

    public virtual DbSet<RandomNewId> RandomNewIds { get; set; }

    public virtual DbSet<SysRole> SysRoles { get; set; }

    public virtual DbSet<SysUser> SysUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=INIHOPCX01\\SQLEXPRESS;Database=SMFG_API;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AI");

        modelBuilder.Entity<InwardDump>(entity =>
        {
            entity.ToTable("InwardDump");

            entity.HasIndex(e => e.Lan, "LAN_INdex");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcknowledgeDate).HasColumnType("datetime");
            entity.Property(e => e.AppRefNo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AwbentryBy).HasColumnName("AWBEntryBy");
            entity.Property(e => e.AwbentryDate)
                .HasColumnType("datetime")
                .HasColumnName("AWBEntryDate");
            entity.Property(e => e.BranchCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BranchName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CartonNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ChequeNo1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("chequeNo1");
            entity.Property(e => e.ChequeNo2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("chequeNo2");
            entity.Property(e => e.ChequeNo3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("chequeNo3");
            entity.Property(e => e.ChequeNo4)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("chequeNo4");
            entity.Property(e => e.ChequeNo5)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("chequeNo5");
            entity.Property(e => e.CourierName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DespatchedDate).HasColumnType("datetime");
            entity.Property(e => e.Disperseddate)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DocumentType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Dpn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DPN");
            entity.Property(e => e.FileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroupId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GroupID");
            entity.Property(e => e.InwardDate).HasColumnType("datetime");
            entity.Property(e => e.IsSync)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .IsFixedLength();
            entity.Property(e => e.Kyc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("KYC");
            entity.Property(e => e.Kyccoapplicant)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("KYCCoapplicant");
            entity.Property(e => e.Lan)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LAN");
            entity.Property(e => e.LoanAmount)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Mip)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MIP");
            entity.Property(e => e.PoddetailsBy).HasColumnName("PODDetailsBy");
            entity.Property(e => e.PoddetailsDate)
                .HasColumnType("datetime")
                .HasColumnName("PODDetailsDate");
            entity.Property(e => e.Podno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PODNo");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RecordType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Received");
            entity.Property(e => e.Remark)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Repaymentsheet)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SyncDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<InwardFile>(entity =>
        {
            entity.HasIndex(e => e.Lan, "LAN_INX");

            entity.HasIndex(e => e.BatchNo, "inx_bATCHnO");

            entity.HasIndex(e => e.DocumentType, "inx_doctype");

            entity.HasIndex(e => e.FileNo, "inx_fileno");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcknowledgeDate).HasColumnType("datetime");
            entity.Property(e => e.AwbentryBy).HasColumnName("AWBEntryBy");
            entity.Property(e => e.AwbentryDate)
                .HasColumnType("datetime")
                .HasColumnName("AWBEntryDate");
            entity.Property(e => e.BatchNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CartonNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ChequeNo1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("chequeNo1");
            entity.Property(e => e.ChequeNo2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("chequeNo2");
            entity.Property(e => e.ChequeNo3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("chequeNo3");
            entity.Property(e => e.ChequeNo4)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("chequeNo4");
            entity.Property(e => e.ChequeNo5)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("chequeNo5");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dpn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DPN");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FileStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InwardDate).HasColumnType("datetime");
            entity.Property(e => e.Kyc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("KYC");
            entity.Property(e => e.Kyccoapplicant)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("KYCCoapplicant");
            entity.Property(e => e.Lan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAN");
            entity.Property(e => e.Mip)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MIP");
            entity.Property(e => e.PoddetailsBy).HasColumnName("PODDetailsBy");
            entity.Property(e => e.PoddetailsDate)
                .HasColumnType("datetime")
                .HasColumnName("PODDetailsDate");
            entity.Property(e => e.Podno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PODNo");
            entity.Property(e => e.RecordType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remark)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Repaymentsheet)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<RandomNewId>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RandomNewID");

            entity.Property(e => e.NewId).HasColumnName("NewID");
        });

        modelBuilder.Entity<SysRole>(entity =>
        {
            entity.ToTable("sysRole");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PageId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("page_id");
            entity.Property(e => e.PageRights)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("page_rights");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("remarks");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("roleName");
            entity.Property(e => e.TaggingRights)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SysUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_sysUSer");

            entity.ToTable("sysUser");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.LoginPass)
                .HasMaxLength(300)
                .HasColumnName("loginPass");
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mobile");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Pwd)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pwd");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("remarks");
            entity.Property(e => e.SysRoleId).HasColumnName("sysRoleID");
            entity.Property(e => e.UserToken)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValueSql("((123123))")
                .HasColumnName("User_Token");
            entity.Property(e => e.Userid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("userid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
