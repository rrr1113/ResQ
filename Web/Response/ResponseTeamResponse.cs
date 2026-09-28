namespace Web.Response;

public record ResponseTeamResponse(
    string Name,
    int NumberOfMembers,
    string Status,
    String Address,
    String City,
    String Country,
    String Type
    );

public record ResponseTeamBasicResponse(
    string Name,
    int NumberOfMembers,
    string Status
    );