using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Lagalt.Models
{
	public class LagaltContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<Applicant> Applicants { get; set; }
		public LagaltContext([NotNullAttribute] DbContextOptions options) : base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().Property(e => e.History)
			.HasConversion(
				v => string.Join(',', v),
				v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

			modelBuilder.Entity<User>().HasData(
				new User {
					Id = "1",
					Username = "Test Knight",
					IsPublic = true,
					History = new string[] { "1", "1", "2" }
				},

				new User {
					Id = "2",
					Username = "Test Prince",
					IsPublic = true,
					History = new string[] {"1", "2", "2"}
				});

			

			modelBuilder.Entity<Owner>().HasData(
				new Owner { Id = 1, Username = "Test Owner1" },
				new Owner { Id = 2, Username = "Test Owner2" }
	);
			modelBuilder.Entity<User>().Property(p => p.Skills)
				.HasConversion(
				v => string.Join(',', v),
				v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

			modelBuilder.Entity<Project>().Property(p => p.Skillset)
				.HasConversion(
				v => string.Join(',', v),
				v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

			modelBuilder.Entity<Applicant>().Property(a => a.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<Project>().HasData(
			new Project 
			{
				Id = 1,
				Title = "My awesome song",
				Description = "A cool song",
				IsPublic = true,
				Field = FieldType.Music,
				Progress = "In progress",
				Skillset = new string[] { "Music production", "Guitar", "Drums", "Singing", "Mixing" },
				Owner = "1"
			},
			new Project
			{
				Id = 2,
				Title = "Runescape",
				Description = "A cool project 3",
				IsPublic = true,
				Field = FieldType.GameDevelopment,
				Progress = "In progress",
				Skillset = new string[] { "Unity" },
				Owner = "2"
			});
			modelBuilder.Entity<Applicant>().HasData(
				new Applicant { Id = 1, Username = "Test Applicant1", Letter = "Test", UserId = "1",ProjectID = 1 },
				new Applicant { Id = 2, Username = "Test Applicant2", Letter = "Test", UserId = "1", ProjectID = 2 }
				);
		}
	}
}
