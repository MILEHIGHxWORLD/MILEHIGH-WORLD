using NUnit.Framework;
using Milehigh.Data;

namespace Milehigh.Tests.Data
{
    [TestFixture]
    public class DialogueTests
    {
        [Test]
        public void IsValid_WithValidData_ReturnsTrue()
        {
            var dialogue = new Dialogue { speaker = "Delilah", text = "The void calls." };
            Assert.IsTrue(dialogue.IsValid());
        }

        [Test]
        public void IsValid_WithEmptySpeaker_ReturnsTrue()
        {
            var dialogue = new Dialogue { speaker = "", text = "Ambient noise." };
            Assert.IsTrue(dialogue.IsValid());
        }

        [Test]
        public void IsValid_WithNullSpeaker_ReturnsTrue()
        {
            var dialogue = new Dialogue { speaker = null!, text = "System message." };
            Assert.IsTrue(dialogue.IsValid());
        }

        [Test]
        public void IsValid_WithSpeakerTooLong_ReturnsFalse()
        {
            var dialogue = new Dialogue { speaker = new string('A', 65), text = "Valid text" };
            Assert.IsFalse(dialogue.IsValid());
        }

        [Test]
        public void IsValid_WithEmptyText_ReturnsFalse()
        {
            var dialogue = new Dialogue { speaker = "Speaker", text = "" };
            Assert.IsFalse(dialogue.IsValid());
        }

        [Test]
        public void IsValid_WithNullText_ReturnsFalse()
        {
            var dialogue = new Dialogue { speaker = "Speaker", text = null! };
            Assert.IsFalse(dialogue.IsValid());
        }

        [Test]
        public void IsValid_WithTextTooLong_ReturnsFalse()
        {
            var dialogue = new Dialogue { speaker = "Speaker", text = new string('B', 1025) };
            Assert.IsFalse(dialogue.IsValid());
        }

        [Test]
        public void IsValid_WithBoundaryValues_ReturnsTrue()
        {
            var dialogue = new Dialogue {
                speaker = new string('A', 64),
                text = new string('B', 1024)
            };
            Assert.IsTrue(dialogue.IsValid());
        }
    }
}
