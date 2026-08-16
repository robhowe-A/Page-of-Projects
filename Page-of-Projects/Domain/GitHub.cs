// --Copyright (c) 2026 Robert A. Howell

using System.Text.Json.Serialization;

namespace ProjectsPage.Domain;

#pragma warning disable CS8618
public class GitHubCollaborator
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("node_id")] public string NodeId { get; set; }

    [JsonPropertyName("repository")] public Repository Repository { get; set; }

    [JsonPropertyName("invitee")] public GitHubUser Invitee { get; set; }

    [JsonPropertyName("inviter")] public GitHubUser Inviter { get; set; }

    [JsonPropertyName("permissions")] public string Permissions { get; set; }

    [JsonPropertyName("created_at")] public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("html_url")] public string HtmlUrl { get; set; }
};

public class Repository
{
    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("node_id")] public string NodeId { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("full_name")] public string FullName { get; set; }

    [JsonPropertyName("private")] public bool Private { get; set; }

    [JsonPropertyName("owner")] public GitHubUser Owner { get; set; }

    [JsonPropertyName("html_url")] public string HtmlUrl { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("fork")] public bool Fork { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("forks_url")] public string ForksUrl { get; set; }

    [JsonPropertyName("keys_url")] public string KeysUrl { get; set; }

    [JsonPropertyName("collaborators_url")]
    public string CollaboratorsUrl { get; set; }

    [JsonPropertyName("teams_url")] public string TeamsUrl { get; set; }

    [JsonPropertyName("hooks_url")] public string HooksUrl { get; set; }

    [JsonPropertyName("issue_events_url")] public string IssueEventsUrl { get; set; }

    [JsonPropertyName("events_url")] public string EventsUrl { get; set; }

    [JsonPropertyName("assignees_url")] public string AssigneesUrl { get; set; }

    [JsonPropertyName("branches_url")] public string BranchesUrl { get; set; }

    [JsonPropertyName("tags_url")] public string TagsUrl { get; set; }

    [JsonPropertyName("blobs_url")] public string BlobsUrl { get; set; }

    [JsonPropertyName("git_tags_url")] public string GitTagsUrl { get; set; }

    [JsonPropertyName("git_refs_url")] public string GitRefsUrl { get; set; }

    [JsonPropertyName("trees_url")] public string TreesUrl { get; set; }

    [JsonPropertyName("statuses_url")] public string StatusesUrl { get; set; }

    [JsonPropertyName("languages_url")] public string LanguagesUrl { get; set; }

    [JsonPropertyName("stargazers_url")] public string StargazersUrl { get; set; }

    [JsonPropertyName("contributors_url")] public string ContributorsUrl { get; set; }

    [JsonPropertyName("subscribers_url")] public string SubscribersUrl { get; set; }

    [JsonPropertyName("subscription_url")] public string SubscriptionUrl { get; set; }

    [JsonPropertyName("commits_url")] public string CommitsUrl { get; set; }

    [JsonPropertyName("git_commits_url")] public string GitCommitsUrl { get; set; }

    [JsonPropertyName("comments_url")] public string CommentsUrl { get; set; }

    [JsonPropertyName("issue_comment_url")]
    public string IssueCommentUrl { get; set; }

    [JsonPropertyName("contents_url")] public string ContentsUrl { get; set; }

    [JsonPropertyName("compare_url")] public string CompareUrl { get; set; }

    [JsonPropertyName("merges_url")] public string MergesUrl { get; set; }

    [JsonPropertyName("archive_url")] public string ArchiveUrl { get; set; }

    [JsonPropertyName("downloads_url")] public string DownloadsUrl { get; set; }

    [JsonPropertyName("issues_url")] public string IssuesUrl { get; set; }

    [JsonPropertyName("pulls_url")] public string PullsUrl { get; set; }

    [JsonPropertyName("milestones_url")] public string MilestonesUrl { get; set; }

    [JsonPropertyName("notifications_url")]
    public string NotificationsUrl { get; set; }

    [JsonPropertyName("labels_url")] public string LabelsUrl { get; set; }

    [JsonPropertyName("releases_url")] public string ReleasesUrl { get; set; }

    [JsonPropertyName("deployments_url")] public string DeploymentsUrl { get; set; }
};
public class GitHubUser
{
    [JsonPropertyName("login")] public string Login { get; set; }

    [JsonPropertyName("id")] public long Id { get; set; }

    [JsonPropertyName("node_id")] public string NodeId { get; set; }

    [JsonPropertyName("avatar_url")] public string AvatarUrl { get; set; }

    [JsonPropertyName("gravatar_id")] public string GravatarId { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("html_url")] public string HtmlUrl { get; set; }

    [JsonPropertyName("followers_url")] public string FollowersUrl { get; set; }

    [JsonPropertyName("following_url")] public string FollowingUrl { get; set; }

    [JsonPropertyName("gists_url")] public string GistsUrl { get; set; }

    [JsonPropertyName("starred_url")] public string StarredUrl { get; set; }

    [JsonPropertyName("subscriptions_url")]
    public string SubscriptionsUrl { get; set; }

    [JsonPropertyName("organizations_url")]
    public string OrganizationsUrl { get; set; }

    [JsonPropertyName("repos_url")] public string ReposUrl { get; set; }

    [JsonPropertyName("events_url")] public string EventsUrl { get; set; }

    [JsonPropertyName("received_events_url")]
    public string ReceivedEventsUrl { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("user_view_type")] public string UserViewType { get; set; }

    [JsonPropertyName("site_admin")] public bool SiteAdmin { get; set; }
};

#pragma warning restore CS8618
