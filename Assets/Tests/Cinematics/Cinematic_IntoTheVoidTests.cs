using NUnit.Framework;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.TestTools;

namespace Milehigh.Tests.Cinematics
{
    public class Cinematic_IntoTheVoidTests
    {
        private Cinematic_IntoTheVoid _cinematic;
        private GameObject _testObject;

        [SetUp]
        public void SetUp()
        {
            _testObject = new GameObject();
            _cinematic = _testObject.AddComponent<Cinematic_IntoTheVoid>();

            // Setup required UI components (mocked/stubbed by creating real ones in Unity context)
            GameObject canvas = new GameObject("Canvas");
            GameObject dialogueBox = new GameObject("DialogueBox");
            dialogueBox.transform.SetParent(canvas.transform);

            GameObject speakerTextObj = new GameObject("SpeakerNameText");
            speakerTextObj.transform.SetParent(dialogueBox.transform);
            _cinematic.SpeakerNameText = speakerTextObj.AddComponent<TextMeshProUGUI>();

            GameObject dialogueTextObj = new GameObject("DialogueText");
            dialogueTextObj.transform.SetParent(dialogueBox.transform);
            _cinematic.DialogueText = dialogueTextObj.AddComponent<TextMeshProUGUI>();

            _cinematic.DialogueBox = dialogueBox;

            _cinematic.baseTypingSpeed = 0.03f;
            _cinematic.kaiSpeedMultiplier = 3.0f;
            _cinematic.skyixSpeedMultiplier = 1.2f;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_testObject);
        }

        [Test]
        public void GetSpeedMultiplier_Kai_ReturnsCorrectMultiplier()
        {
            float multiplier = _cinematic.GetSpeedMultiplier("Kai");
            Assert.AreEqual(3.0f, multiplier);
        }

        [Test]
        public void GetSpeedMultiplier_Skyix_ReturnsCorrectMultiplier()
        {
            float multiplier = _cinematic.GetSpeedMultiplier("Sky.ix");
            Assert.AreEqual(1.2f, multiplier);
        }

        [Test]
        public void GetSpeedMultiplier_Default_ReturnsOne()
        {
            float multiplier = _cinematic.GetSpeedMultiplier("Unknown");
            Assert.AreEqual(1.0f, multiplier);
        }

        [Test]
        public void GetSpeakerColor_Skyix_ReturnsCyan()
        {
            Color color = _cinematic.GetSpeakerColor("Sky.ix");
            Assert.AreEqual(Color.cyan, color);
        }

        [Test]
        public void GetSpeakerColor_Kai_ReturnsGold()
        {
            Color color = _cinematic.GetSpeakerColor("Kai");
            // Gold: 1f, 0.84f, 0f
            Assert.AreEqual(new Color(1f, 0.84f, 0f), color);
        }

        [Test]
        public void GetSpeakerColor_Delilah_ReturnsVoidPurple()
        {
            Color color = _cinematic.GetSpeakerColor("Delilah");
            // Void Purple: 0.6f, 0.1f, 0.9f
            Assert.AreEqual(new Color(0.6f, 0.1f, 0.9f), color);
        }

        [Test]
        public void GetSpeakerColor_Default_ReturnsWhite()
        {
            Color color = _cinematic.GetSpeakerColor("SomeoneElse");
            Assert.AreEqual(Color.white, color);
        }
    }
}
