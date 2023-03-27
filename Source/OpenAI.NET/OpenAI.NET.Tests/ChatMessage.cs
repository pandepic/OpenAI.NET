using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAINET.Tests
{
    [TestFixture]
    public class ChatMessage_Tests
    {
        public const string TEST_MESSAGE = "hello world";

        [Test]
        public void FromSystem()
        {
            var message = ChatMessage.FromSystem(TEST_MESSAGE);

            Assert.That(message.Role, Is.EqualTo(ChatMessageRole.System));
            Assert.That(message.Message, Is.EqualTo(TEST_MESSAGE));
        }

        [Test]
        public void FromUser()
        {
            var message = ChatMessage.FromUser(TEST_MESSAGE);

            Assert.That(message.Role, Is.EqualTo(ChatMessageRole.User));
            Assert.That(message.Message, Is.EqualTo(TEST_MESSAGE));
        }

        [Test]
        public void FromAssistant()
        {
            var message = ChatMessage.FromAssistant(TEST_MESSAGE);

            Assert.That(message.Role, Is.EqualTo(ChatMessageRole.Assistant));
            Assert.That(message.Message, Is.EqualTo(TEST_MESSAGE));
        }
    }
}
