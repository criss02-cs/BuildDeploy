using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BuildDeployWpf.Messages;

public class Appearing(string value) : ValueChangedMessage<string>(value)
{
    public Appearing() : this("") {}
}
