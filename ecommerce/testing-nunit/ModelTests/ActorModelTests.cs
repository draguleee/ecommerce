using NUnit.Framework;
using ecommerce.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework.Legacy;

namespace ecommerce.Tests.Models
{
    [TestFixture]
    public class ActorModelTests
    {
        private Actor _actor;

        [SetUp]
        public void Setup()
        {
            _actor = new Actor
            {
                ProfilePictureUrl = "https://example.com/image.jpg",
                FullName = "John Doe",
                Bio = "An experienced actor known for versatile roles.",
                Actors_Movies = new List<Actor_Movie>()
            };
        }

        // Result validation
        private IList<ValidationResult> ValidateModel(Actor actor)
        {
            var validationContext = new ValidationContext(actor);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(actor, validationContext, validationResults, true);
            return validationResults;
        }

        [Test]
        public void Actor_WithValidData_ShouldBeValid()
        {
            // Act
            var result = ValidateModel(_actor);

            // Assert
            ClassicAssert.IsEmpty(result);
        }

        [Test]
        public void Actor_WithoutProfilePicture_ShouldBeInvalid()
        {
            // Arrange
            _actor.ProfilePictureUrl = null;

            // Act
            var result = ValidateModel(_actor);

            // Assert
            ClassicAssert.IsTrue(result.Any(v => v.ErrorMessage == "Profile Picture is required!"));
        }

        [Test]
        public void Actor_WithoutFullName_ShouldBeInvalid()
        {
            // Arrange
            _actor.FullName = null;

            // Act
            var result = ValidateModel(_actor);

            // Assert
            ClassicAssert.IsTrue(result.Any(v => v.ErrorMessage == "Full Name is required!"));
        }

        [Test]
        public void Actor_WithShortFullName_ShouldBeInvalid()
        {
            // Arrange
            _actor.FullName = "Jo";

            // Act
            var result = ValidateModel(_actor);

            // Assert
            ClassicAssert.IsTrue(result.Any(v => v.ErrorMessage == "Full Name must be between 3 and 50 chars!"));
        }

        [Test]
        public void Actor_WithLongFullName_ShouldBeInvalid()
        {
            // Arrange
            _actor.FullName = new string('a', 51);

            // Act
            var result = ValidateModel(_actor);

            // Assert
            ClassicAssert.IsTrue(result.Any(v => v.ErrorMessage == "Full Name must be between 3 and 50 chars!"));
        }

        [Test]
        public void Actor_WithoutBio_ShouldBeInvalid()
        {
            // Arrange
            _actor.Bio = null;

            // Act
            var result = ValidateModel(_actor);

            // Assert
            ClassicAssert.IsTrue(result.Any(v => v.ErrorMessage == "Biography is required!"));
        }
    }
}
