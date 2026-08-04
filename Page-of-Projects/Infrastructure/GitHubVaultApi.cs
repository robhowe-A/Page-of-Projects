// --Copyright (c) 2026 Robert A. Howell

using System.Text;
using System.Text.RegularExpressions;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace ProjectsPage.Infrastructure;

public class GitHubVaultApi
{
    public void EnsureClientUsernameKeyVaultAddition(Collaborator collaborator)
    {
        //Ensuring the client username is added to the Key Vaule
        switch (GetKeyVaultSecret(collaborator.ClientUsernameKeyVaultKey))
        {
            case { HasValue: false }:
                AddKeyVaultSecret(collaborator.ClientUsernameKeyVaultKey, collaborator.ClientUsernameValue);
                break;
            case { HasValue: true }:
                WriteLine("INFO: Client key already exists in Key Vault.");
                break;
            case null:
                AddKeyVaultSecret(collaborator.ClientUsernameKeyVaultKey, collaborator.ClientUsernameValue);
                break;
        }
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
        request.Headers.Add("User-Agent", "ProjectsPageApp-AccessController|roberthowell.dev|Projects Hub|A portfolio development by Robert Howell");
        request.Version = new Version(2, 0);
        request.VersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
        request.Content = new StringContent("""{"permission":"pull"}""", Encoding.UTF8);

        return request;
    }

    private Response<KeyVaultSecret>? GetKeyVaultSecret(string secretKey) => GetVaultSecret(secretKey);
    private void AddKeyVaultSecret(string secretKey, string secretValue) => SetKeyVaultValue(secretKey, secretValue);

    private const string KeyVaultOwnerKey = @"GitHub-All-collaborator-owner";
    private const string KeyVaultCollaboratorTokenKey = @"GitHub-All-collaborator-token";
    private KeyVaultSecret KeyVaultOwnerSecret { get; init; }
    private KeyVaultSecret KeyVaultCollaboratorTokenSecret { get; init; }

    private const string KeyVaultVaultUri = @"https://keyvaultpremiumonly.vault.azure.net/";

    private SecretClient SecretClient { get; init; } 

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

            var sanitizedRepositoryName = SanitizeRepositoryName(repositoryName);
            RepositoryName = repositoryName;

            ClientUsernameKeyVaultKey = $"GitHub-{sanitizedRepositoryName}-collaborator-{ClientUsernameIdentifier}";
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

        private static string SanitizeRepositoryName(string repositoryName)
        {
            //Key vault names cannot contain '_' characters
            return repositoryName.Replace("_", "--");
        }

        private void ReaffirmUsernameIsValid(string clientUsernameValue)
        {
            IsValidUsername = CheckUsernameIsValid(clientUsernameValue);
        }
    }

    public GitHubVaultApi()
    {
        SecretClient = new SecretClient(vaultUri: new Uri(KeyVaultVaultUri), credential: new DefaultAzureCredential());
        KeyVaultOwnerSecret = (
                GetVaultSecret(KeyVaultOwnerKey) ?? throw new NullReferenceException("ERROR: Missing collaborator key in Key Vault..")
        ).Value;

        KeyVaultCollaboratorTokenSecret = (
                GetVaultSecret(KeyVaultCollaboratorTokenKey) ?? throw new NullReferenceException("ERROR: Missing token key in Key Vault.")
        ).Value;
    }

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
