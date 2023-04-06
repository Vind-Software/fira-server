namespace FiraServer.api.Common;

[Flags]
public enum ApiScopeVisibilities {
    Admin       = 0b_0000_0000,
    Institution = 0b_0000_0001,
    User        = 0b_0000_0010,
    Public      = 0b_1000_0000
}