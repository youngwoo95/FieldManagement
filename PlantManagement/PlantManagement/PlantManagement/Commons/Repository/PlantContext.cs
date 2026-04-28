using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PlantManagement.Commons.DBModels;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PlantManagement.Commons.Repository;

public partial class PlantContext : DbContext
{
    public PlantContext()
    {
    }

    public PlantContext(DbContextOptions<PlantContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CustomerTb> CustomerTbs { get; set; }

    public virtual DbSet<FacilityLogTb> FacilityLogTbs { get; set; }

    public virtual DbSet<FacilityTb> FacilityTbs { get; set; }

    public virtual DbSet<FloorTb> FloorTbs { get; set; }

    public virtual DbSet<NoticeTb> NoticeTbs { get; set; }

    public virtual DbSet<OrderTb> OrderTbs { get; set; }

    public virtual DbSet<ProductionTb> ProductionTbs { get; set; }

    public virtual DbSet<WorkTb> WorkTbs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=100.127.26.53;port=3306;database=PlantManagement;user=rladyddn258;password=rladyddn!!95", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.45-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<CustomerTb>(entity =>
        {
            entity.HasKey(e => e.CustomerSeq).HasName("PRIMARY");

            entity.ToTable("CustomerTb", tb => tb.HasComment("고객사 테이블"));

            entity.Property(e => e.CustomerSeq)
                .HasComment("고객사PK")
                .HasColumnName("customerSeq");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasComment("주소")
                .HasColumnName("address");
            entity.Property(e => e.CreateDt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createDt");
            entity.Property(e => e.DelYn)
                .HasComment("삭제여부")
                .HasColumnName("delYn");
            entity.Property(e => e.Department)
                .HasMaxLength(255)
                .HasComment("부서명")
                .HasColumnName("department");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasComment("이메일")
                .HasColumnName("email");
            entity.Property(e => e.Gubun)
                .HasMaxLength(255)
                .HasComment("고객구분")
                .HasColumnName("gubun");
            entity.Property(e => e.Manager)
                .HasMaxLength(255)
                .HasComment("담당자명")
                .HasColumnName("manager");
            entity.Property(e => e.Memo)
                .HasMaxLength(255)
                .HasComment("비고")
                .HasColumnName("memo");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("고객사 명")
                .HasColumnName("name");
            entity.Property(e => e.Tel)
                .HasMaxLength(255)
                .HasComment("연락처")
                .HasColumnName("tel");
        });

        modelBuilder.Entity<FacilityLogTb>(entity =>
        {
            entity.HasKey(e => e.LogSeq).HasName("PRIMARY");

            entity.ToTable("FacilityLogTb", tb => tb.HasComment("설비이력"));

            entity.HasIndex(e => e.FacilitySeq, "FK_FacilityLogTb_FacilityTb");

            entity.HasIndex(e => e.WorkSeq, "FK_FacilityLogTb_WorkTb");

            entity.Property(e => e.LogSeq)
                .HasComment("로그 PK")
                .HasColumnName("logSeq");
            entity.Property(e => e.CreateDt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createDt");
            entity.Property(e => e.FacilitySeq)
                .HasComment("설비 FK")
                .HasColumnName("facilitySeq");
            entity.Property(e => e.Memo)
                .HasMaxLength(255)
                .HasComment("메모")
                .HasColumnName("memo");
            entity.Property(e => e.WorkSeq)
                .HasComment("작업지시번호 FK")
                .HasColumnName("workSeq");

            entity.HasOne(d => d.FacilitySeqNavigation).WithMany(p => p.FacilityLogTbs)
                .HasForeignKey(d => d.FacilitySeq)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacilityLogTb_FacilityTb");

            entity.HasOne(d => d.WorkSeqNavigation).WithMany(p => p.FacilityLogTbs)
                .HasForeignKey(d => d.WorkSeq)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacilityLogTb_WorkTb");
        });

        modelBuilder.Entity<FacilityTb>(entity =>
        {
            entity.HasKey(e => e.FacilitySeq).HasName("PRIMARY");

            entity.ToTable("FacilityTb", tb => tb.HasComment("설비 테이블"));

            entity.Property(e => e.FacilitySeq)
                .HasComment("설비 PK")
                .HasColumnName("facilitySeq");
            entity.Property(e => e.CreateTb)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("생성일")
                .HasColumnType("datetime")
                .HasColumnName("createTb");
            entity.Property(e => e.DelYn)
                .HasComment("삭제여부")
                .HasColumnName("delYn");
            entity.Property(e => e.FacilityName)
                .HasMaxLength(255)
                .HasComment("설비명")
                .HasColumnName("facilityName");
            entity.Property(e => e.Maker)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasComment("제조사")
                .HasColumnName("maker");
            entity.Property(e => e.Purpose)
                .HasMaxLength(255)
                .HasComment("용도")
                .HasColumnName("purpose");
        });

        modelBuilder.Entity<FloorTb>(entity =>
        {
            entity.HasKey(e => e.FloorSeq).HasName("PRIMARY");

            entity.ToTable("FloorTb", tb => tb.HasComment("층정보 테이블"));

            entity.HasIndex(e => e.FacilitySeq, "FK__FacilityTb");

            entity.HasIndex(e => e.Name, "uk_floorName").IsUnique();

            entity.Property(e => e.FloorSeq)
                .HasComment("PK")
                .HasColumnName("floorSeq");
            entity.Property(e => e.Attach)
                .HasMaxLength(255)
                .HasComment("첨부파일")
                .HasColumnName("attach");
            entity.Property(e => e.FacilitySeq)
                .HasComment("FK")
                .HasColumnName("facilitySeq");
            entity.Property(e => e.Name)
                .HasComment("층이름")
                .HasColumnName("name");

            entity.HasOne(d => d.FacilitySeqNavigation).WithMany(p => p.FloorTbs)
                .HasForeignKey(d => d.FacilitySeq)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FacilityTb");
        });

        modelBuilder.Entity<NoticeTb>(entity =>
        {
            entity.HasKey(e => e.NoticeSeq).HasName("PRIMARY");

            entity.ToTable("NoticeTb", tb => tb.HasComment("공지사항 테이블"));

            entity.Property(e => e.NoticeSeq).HasColumnName("noticeSeq");
            entity.Property(e => e.CreateDt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createDt");
            entity.Property(e => e.CreateUser)
                .HasMaxLength(255)
                .HasComment("작성자")
                .HasColumnName("createUser");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasComment("설명")
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasComment("제목")
                .HasColumnName("title");
        });

        modelBuilder.Entity<OrderTb>(entity =>
        {
            entity.HasKey(e => e.OrderSeq).HasName("PRIMARY");

            entity.ToTable("OrderTb", tb => tb.HasComment("수주테이블"));

            entity.HasIndex(e => e.CustomerSeq, "FK_OrderTb_MakerTb");

            entity.Property(e => e.OrderSeq)
                .HasComment("수주 PK")
                .HasColumnName("orderSeq");
            entity.Property(e => e.Attach)
                .HasMaxLength(255)
                .HasComment("첨부파일")
                .HasColumnName("attach");
            entity.Property(e => e.CustomerSeq)
                .HasComment("고객사 FK")
                .HasColumnName("customerSeq");
            entity.Property(e => e.DelYn)
                .HasComment("삭제여부")
                .HasColumnName("delYn");
            entity.Property(e => e.EndDt)
                .HasComment("마감일")
                .HasColumnName("endDt");
            entity.Property(e => e.OrderQty)
                .HasComment("요청수량")
                .HasColumnName("orderQty");
            entity.Property(e => e.StartDt)
                .HasComment("요청일")
                .HasColumnName("startDt");

            entity.HasOne(d => d.CustomerSeqNavigation).WithMany(p => p.OrderTbs)
                .HasForeignKey(d => d.CustomerSeq)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderTb_MakerTb");
        });

        modelBuilder.Entity<ProductionTb>(entity =>
        {
            entity.HasKey(e => e.ProductionSeq).HasName("PRIMARY");

            entity.ToTable("ProductionTb", tb => tb.HasComment("생산 테이블"));

            entity.HasIndex(e => e.WorkerSeq, "FK_ProductionTb_WorkTb");

            entity.Property(e => e.ProductionSeq).HasColumnName("productionSeq");
            entity.Property(e => e.CreateDt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createDt");
            entity.Property(e => e.Status)
                .HasComment("제품상태 1: 정상 0:불량")
                .HasColumnName("status");
            entity.Property(e => e.WorkerSeq)
                .HasDefaultValueSql("'0'")
                .HasComment("작업지시번호")
                .HasColumnName("workerSeq");

            entity.HasOne(d => d.WorkerSeqNavigation).WithMany(p => p.ProductionTbs)
                .HasForeignKey(d => d.WorkerSeq)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductionTb_WorkTb");
        });

        modelBuilder.Entity<WorkTb>(entity =>
        {
            entity.HasKey(e => e.WorkSeq).HasName("PRIMARY");

            entity.ToTable("WorkTb", tb => tb.HasComment("작업 테이블"));

            entity.HasIndex(e => e.FacilitySeq, "FK_WorkTb_FacilityTb");

            entity.HasIndex(e => e.OrderSeq, "FK_WorkTb_OrderTb");

            entity.Property(e => e.WorkSeq)
                .HasComment("작업지시번호 PK")
                .HasColumnName("workSeq");
            entity.Property(e => e.CurrentQty)
                .HasComment("현재수량")
                .HasColumnName("currentQty");
            entity.Property(e => e.DelYn).HasColumnName("delYn");
            entity.Property(e => e.EndWorkDt)
                .HasComment("작업 종료일")
                .HasColumnType("datetime")
                .HasColumnName("endWorkDt");
            entity.Property(e => e.FacilitySeq)
                .HasComment("설비 FK")
                .HasColumnName("facilitySeq");
            entity.Property(e => e.OrderSeq)
                .HasComment("수주 FK")
                .HasColumnName("orderSeq");
            entity.Property(e => e.StartWorkDt)
                .HasComment("작업 시작일")
                .HasColumnType("datetime")
                .HasColumnName("startWorkDt");
            entity.Property(e => e.Status)
                .HasComment("현재 상태")
                .HasColumnName("status");

            entity.HasOne(d => d.FacilitySeqNavigation).WithMany(p => p.WorkTbs)
                .HasForeignKey(d => d.FacilitySeq)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkTb_FacilityTb");

            entity.HasOne(d => d.OrderSeqNavigation).WithMany(p => p.WorkTbs)
                .HasForeignKey(d => d.OrderSeq)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkTb_OrderTb");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
