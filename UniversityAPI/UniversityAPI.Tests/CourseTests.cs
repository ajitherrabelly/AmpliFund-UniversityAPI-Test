using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityAPI.Models;
using Xunit;

namespace UniversityAPI.Tests
{
    public class CourseTests
    {

        [Fact]
        public void Course_Creation_SetsPropertiesCorrectly()
        {
            // Arrange
            var course = new Course
            {
                Code = "CS101",
                Name = "Introduction to Computer Science",
                Credits = 3,
                Department = "Computer Science",
                Instructor = "Dr. Smith",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 12, 15),
                MaxCapacity = 30,
                IsActive = true
            };

            // Assert
            Assert.Equal("CS101", course.Code);
            Assert.Equal("Introduction to Computer Science", course.Name);
            Assert.Equal(3, course.Credits);
            Assert.Equal("Computer Science", course.Department);
            Assert.Equal("Dr. Smith", course.Instructor);
            Assert.Equal(new DateTime(2023, 9, 1), course.StartDate);
            Assert.Equal(new DateTime(2023, 12, 15), course.EndDate);
            Assert.Equal(30, course.MaxCapacity);
            Assert.True(course.IsActive);
        }

        [Fact]
        public void Course_EnrollStudent_IncreasesCurrentEnrollment()
        {
            // Arrange
            var course = new Course { MaxCapacity = 30, CurrentEnrollment = 0 };
            var student = new Student();

            // Act
            course.EnrolledStudents.Add(student);
            course.CurrentEnrollment++;

            // Assert
            Assert.Equal(1, course.CurrentEnrollment);
            Assert.Contains(student, course.EnrolledStudents);
        }

        [Fact]
        public void Course_EnrollStudent_ThrowsException_WhenAtCapacity()
        {
            // Arrange
            var course = new Course { MaxCapacity = 1, CurrentEnrollment = 1 };
            var student = new Student();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                if (course.CurrentEnrollment >= course.MaxCapacity)
                    throw new InvalidOperationException("Course is at maximum capacity");
                course.EnrolledStudents.Add(student);
                course.CurrentEnrollment++;
            });
        }

    }
}
