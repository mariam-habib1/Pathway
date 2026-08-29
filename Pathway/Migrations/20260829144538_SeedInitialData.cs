using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pathway.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Build modern websites and web apps", "Web Development" },
                    { 2, "Protect systems and data from threats", "Cyber Security" },
                    { 3, "Machine learning and AI fundamentals", "Artificial Intelligence" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "Name", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ahmed.instructor@pathway.com", "Ahmed Hassan", "TEMP_HASH_1", "Instructor" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "salma.instructor@pathway.com", "Salma Adel", "TEMP_HASH_2", "Instructor" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "CategoryId", "CreatedAt", "Description", "InstructorId", "Price", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learn to build full-stack web applications using ASP.NET Core MVC, Entity Framework, and SQL Server.", 1, 499m, "ASP.NET Core MVC from Scratch" },
                    { 2, 2, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "A hands-on introduction to penetration testing, network security, and vulnerability assessment.", 2, 699m, "Ethical Hacking Essentials" },
                    { 3, 3, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Understand the core concepts behind supervised and unsupervised learning with real datasets.", 2, 599m, "Machine Learning Foundations" },
                    { 4, 1, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Build interactive user interfaces with React, hooks, and modern JavaScript.", 1, 449m, "React for Beginners" },
                    { 5, 2, new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learn firewalls, VPNs, and intrusion detection to secure enterprise networks.", 2, 649m, "Network Security Fundamentals" },
                    { 6, 3, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dive into neural networks, CNNs, and practical AI projects using PyTorch.", 2, 749m, "Deep Learning with Python" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);
        }
    }
}
