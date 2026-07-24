// --Copyright (c) 2026 Robert A. Howell

using System.Text;
using System.Text.RegularExpressions;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace ProjectsPage.Infrastructure;

public class GitHubApi
{
    private const string KeyVaultOwnerKey = @"GitHub-All-collaborator-owner";
    private const string KeyVaultCollaboratorTokenKey = @"GitHub-All-collaborator-token";
    private KeyVaultSecret KeyVaultOwnerSecret { get; init; }
    private KeyVaultSecret KeyVaultCollaboratorTokenSecret { get; init; }

    private const string KeyVaultVaultUri = @"https://keyvaultpremiumonly.vault.azure.net/";

    private SecretClient SecretClient { get; init; } 

    public KeyVaultSecret GetKeyVaultOwnerSecret() => KeyVaultOwnerSecret;
    public KeyVaultSecret GetKeyVaultCollaboratorTokenSecret() => KeyVaultCollaboratorTokenSecret;

    public record Collaborator
    {
        public string RepositoryName { get; init; }
        public string ClientUsernameKeyVaultKey { get; init; }
        public string ClientUsernameValue { get; init; }
        private string ClientUsernameIdentifier { get; set; }
        private bool IsValidUsername { get; set; }

        public Collaborator(string repositoryName, string clientUsername)
        {
            if (string.IsNullOrWhiteSpace(repositoryName))
                throw new ArgumentException("Missing a required argument.", nameof(repositoryName));

            if (string.IsNullOrWhiteSpace(clientUsername))
                throw new ArgumentException("Missing a required argument.", nameof(clientUsername));

            ReaffirmUsernameIsValid(clientUsername);
            ClientUsernameIdentifier = NewUsernameIdentifier(clientUsername);

            RepositoryName = repositoryName;

            ClientUsernameKeyVaultKey = $"GitHub-{repositoryName}-collaborator-{ClientUsernameIdentifier}";
            ClientUsernameValue = clientUsername;
        }

        private string NewUsernameIdentifier(string username)
        {
            var today = DateTime.Now;

            var clientUsernameFirstThree = username.Length >= 3 ? username[..3] : username;

            return $"{clientUsernameFirstThree}{today:yyMMdd}";
        }

        public static bool CheckUsernameIsValid(string clientUsernameValue)
        {
            //GitHub usernames can only contain alphanumeric characters or single
            //hyphens, and cannot begin or end with a hyphen.
            var usernameRegex = new Regex(@"^([A-Za-z0-9]{1}[A-Za-z0-9-]*[A-Za-z0-9]{1})$");

            return usernameRegex.IsMatch(clientUsernameValue);
        }

        private void ReaffirmUsernameIsValid(string clientUsernameValue)
        {
            IsValidUsername = CheckUsernameIsValid(clientUsernameValue);
        }
    }

    public GitHubApi()
    {
        SecretClient = new SecretClient(vaultUri: new Uri(KeyVaultVaultUri), credential: new DefaultAzureCredential());
        KeyVaultOwnerSecret = (
                GetVaultSecret(KeyVaultOwnerKey) ?? throw new NullReferenceException("ERROR: Missing collaborator key in Key Vault..")
        ).Value;

        KeyVaultCollaboratorTokenSecret = (
                GetVaultSecret(KeyVaultCollaboratorTokenKey) ?? throw new NullReferenceException("ERROR: Missing token key in Key Vault.")
        ).Value;
    }

    public Uri CollaboratorAdditionEndpointUrl(Collaborator collaborator)
    {
        return new Uri($"https://api.github.com/repos/{KeyVaultOwnerSecret.Value}/{collaborator.RepositoryName}/collaborators/{collaborator.ClientUsernameValue}");
    }

    public HttpRequestMessage CollaboratorAdditionCreateRequest()
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, "");
        request.Headers.Add("Accept", "application/vnd.github+json");
        request.Headers.Add("Authorization", $"Bearer {KeyVaultCollaboratorTokenSecret.Value}");
        request.Headers.Add("X-GitHub-Api-Version", "2026-03-10");
        request.Headers.Add("User-Agent", "ProjectsPageApp-JitController|roberthowell.dev|Projects Hub developed by Robert Howell");
        request.Version = new Version(2, 0);
        request.VersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
        request.Content = new StringContent("""{"permission":"pull"}""", Encoding.UTF8);

        return request;
    }

    public Response<KeyVaultSecret>? GetKeyVaultSecret(string secretKey) => GetVaultSecret(secretKey);
    public void AddKeyVaultSecret(string secretKey, string secretValue) => SetKeyVaultValue(secretKey, secretValue);
    private Response<KeyVaultSecret>? GetVaultSecret(string secretKey)
    {
        try
        {
            return SecretClient.GetSecret(secretKey)
                 ?? throw new NullReferenceException("Error from accessing the Key Vault.");
        }
        catch (RequestFailedException e) when (e.ErrorCode == "SecretNotFound")
        { //Exception message from Azure for when secret is not found
            WriteLine($"WARN: Client username key not found in Key Vault");
            return null;
        }
        catch (Exception e)
        {
            WriteLine("ERROR: A problem occurred getting secret value.");
            throw new ApplicationException($"Error getting secret value: {e.Message}");
        }
    }

    private void SetKeyVaultValue(string clientUsernameKeyName, string clientUsernameValue)
    {
        try
        {
            SecretClient.SetSecret(clientUsernameKeyName, clientUsernameValue);
            WriteLine("INFO: Client key added to the Key Vault.");
        }
        catch (RequestFailedException e) when (e.ErrorCode == "Conflict")
        { //Exception message from Azure for when secret is deleted and not purged
            WriteLine($"WARN: Client username key is deleted in the Key Vault");
            SecretClient.StartRecoverDeletedSecret(clientUsernameKeyName);
            GetVaultSecret(clientUsernameKeyName);
        }
        catch (Exception e)
        {
            WriteLine("ERROR: Client key already exists in Key Vault.");
            throw new ApplicationException($"Error accessing Key Vault: {e.Message}");
        }
    }
};
