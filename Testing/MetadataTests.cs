using Xunit;
using Milehigh.Data;

namespace Milehigh.Tests
{
    public class MetadataTests
    {
        [Fact]
        public void Metadata_ValidVoidSaturation_ReturnsTrue()
        {
            var metadata = new Metadata { voidSaturationLevel = 0.5f };
            Assert.True(metadata.IsValid());
        }

        [Fact]
        public void Metadata_VoidSaturationBoundaryLower_ReturnsTrue()
        {
            var metadata = new Metadata { voidSaturationLevel = 0.0f };
            Assert.True(metadata.IsValid());
        }

        [Fact]
        public void Metadata_VoidSaturationBoundaryUpper_ReturnsTrue()
        {
            var metadata = new Metadata { voidSaturationLevel = 1.0f };
            Assert.True(metadata.IsValid());
        }

        [Fact]
        public void Metadata_VoidSaturationTooLow_ReturnsFalse()
        {
            var metadata = new Metadata { voidSaturationLevel = -0.01f };
            Assert.False(metadata.IsValid());
        }

        [Fact]
        public void Metadata_VoidSaturationTooHigh_ReturnsFalse()
        {
            var metadata = new Metadata { voidSaturationLevel = 1.01f };
            Assert.False(metadata.IsValid());
        }

        [Fact]
        public void Metadata_EnvironmentLengthValid_ReturnsTrue()
        {
            var metadata = new Metadata { voidSaturationLevel = 0.5f, environment = new string('a', 128) };
            Assert.True(metadata.IsValid());
        }

        [Fact]
        public void Metadata_EnvironmentLengthTooLong_ReturnsFalse()
        {
            var metadata = new Metadata { voidSaturationLevel = 0.5f, environment = new string('a', 129) };
            Assert.False(metadata.IsValid());
        }
    }
}
