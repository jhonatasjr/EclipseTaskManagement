﻿// <auto-generated />
using System;
using EclipseWorks.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EclipseWorks.Domain.Migrations
{
    [DbContext(typeof(Contexto))]
    [Migration("20240218233711_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EclipseWorks.Domain.Models.Comentario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DtCreate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("IdUsuario")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TarefaId")
                        .HasColumnType("int");

                    b.Property<string>("Texto")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("TarefaId");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("EclipseWorks.Domain.Models.Historico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Alteracoes")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DtModificacao")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdTarefa")
                        .HasColumnType("int");

                    b.Property<string>("TpModificacao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserAlter")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Historico");
                });

            modelBuilder.Entity("EclipseWorks.Domain.Models.Projeto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("DtAlter")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DtCreate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DtDeleted")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("IdUsuario")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("NmProjeto")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserAlter")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserDeleted")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Projetos");
                });

            modelBuilder.Entity("EclipseWorks.Domain.Models.Tarefa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("DtAlter")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DtConclucao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DtCreate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DtDeleted")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DtVencimento")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdProjeto")
                        .HasColumnType("int");

                    b.Property<string>("IdUsuario")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Prioridade")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserAlter")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserDeleted")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("IdProjeto");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Tarefas");
                });

            modelBuilder.Entity("EclipseWorks.Domain.Models.Usuario", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(200)");

                    b.Property<int>("Funcao")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("EclipseWorks.Domain.Models.Comentario", b =>
                {
                    b.HasOne("EclipseWorks.Domain.Models.Tarefa", null)
                        .WithMany("Comentarios")
                        .HasForeignKey("TarefaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EclipseWorks.Domain.Models.Projeto", b =>
                {
                    b.HasOne("EclipseWorks.Domain.Models.Usuario", "CriadorDoProjeto")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CriadorDoProjeto");
                });

            modelBuilder.Entity("EclipseWorks.Domain.Models.Tarefa", b =>
                {
                    b.HasOne("EclipseWorks.Domain.Models.Projeto", "Projeto")
                        .WithMany("Tarefas")
                        .HasForeignKey("IdProjeto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EclipseWorks.Domain.Models.Usuario", "UsuarioCriacao")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Projeto");

                    b.Navigation("UsuarioCriacao");
                });

            modelBuilder.Entity("EclipseWorks.Domain.Models.Projeto", b =>
                {
                    b.Navigation("Tarefas");
                });

            modelBuilder.Entity("EclipseWorks.Domain.Models.Tarefa", b =>
                {
                    b.Navigation("Comentarios");
                });
#pragma warning restore 612, 618
        }
    }
}
