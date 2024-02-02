using FatCat.Toolkit.Messaging;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Messaging;

public class MessengerSubscribeTests
{
	private bool wasCallbackHit;

	[Fact]
	public void CanRegisterForAMessageAndHitCallback()
	{
		Messenger.Thread = new FakeThread();

		Messenger.Subscribe<TestMessage>(OnTestMessageCallback);

		Messenger.Send(new TestMessage());

		wasCallbackHit.Should().BeTrue();
	}

	private Task OnTestMessageCallback(TestMessage arg)
	{
		wasCallbackHit = true;

		return Task.CompletedTask;
	}

	public class TestMessage : Message { }
}
