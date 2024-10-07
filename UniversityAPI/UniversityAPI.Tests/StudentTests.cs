using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityAPI.Models;
using Xunit;

namespace UniversityAPI.Tests
{
    public class StudentTests
    {
        [Fact]
        public void Student_Creation_SetsPropertiesCorrectly()
        {
            // Arrange
            var student = new Student
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            // Assert
            Assert.Equal("John", student.FirstName);
            Assert.Equal("Doe", student.LastName);
            Assert.Equal("john.doe@example.com", student.Email);
            Assert.Equal(new DateTime(2000, 1, 1), student.DateOfBirth);
        }

        [Fact]
        public void Student_EnrollInCourse_AddsCourseToEnrolledCourses()
        {
            // Arrange
            var student = new Student();
            var course = new Course();

            // Act
            student.EnrolledCourses.Add(course);

            // Assert
            Assert.Contains(course, student.EnrolledCourses);
        }
    }
}
