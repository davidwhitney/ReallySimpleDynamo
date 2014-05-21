using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;

namespace ReallySimpleDynamo.CredentialDetection.Providers
{
    /// <summary>
    /// Credentials that are retrieved from the Instance Profile service on an EC2 instance
    /// </summary>
    public class InstanceProfileAwsCredentials
    {
        // Set preempt expiry to 15 minutes. New access keys are available at least 15 minutes before expiry time.
        // http://docs.aws.amazon.com/IAM/latest/UserGuide/role-usecase-ec2app.html
        private static TimeSpan _preemptExpiryTime = TimeSpan.FromMinutes(15);

        private CredentialsRefreshState _currentRefreshState;
        private CredentialsRefreshState _currentState;

        private static readonly TimeSpan RefreshAttemptPeriod = TimeSpan.FromHours(1);
        private readonly object _refreshLock = new object();

        private static readonly string[] AliasSeparators = { "<br/>" };
        private const string Server = "http://169.254.169.254";
        private const string RolesPath = "/latest/meta-data/iam/security-credentials/";
        private const string InfoPath = "/latest/meta-data/iam/info";
        private const string SuccessCode = "Success";

        private static Uri RolesUri
        {
            get { return new Uri(Server + RolesPath); }
        }
        private Uri CurrentRoleUri
        {
            get { return new Uri(Server + RolesPath + Role); }
        }
        private static Uri InfoUri
        {
            get { return new Uri(Server + InfoPath); }
        }

        public string Role { get; set; }

    
        protected CredentialsRefreshState GenerateNewCredentials()
        {
            CredentialsRefreshState newState = null;
            try
            {
                // Attempt to get early credentials. OK to fail at this point.
                newState = GetRefreshState();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error getting credentials from Instance Profile service: {0}", e);
            }

            // If successful, save new credentials
            if (newState != null)
                _currentRefreshState = newState;

            // If still not successful (no credentials available at start), attempt once more to
            // get credentials, but now without swallowing exception
            if (_currentRefreshState == null)
                _currentRefreshState = GetRefreshState();

            // Return credentials that will expire in at most one hour
            CredentialsRefreshState state = GetEarlyRefreshState(_currentRefreshState);
            return state;
        }
        
        public InstanceProfileAwsCredentials(string role)
        {
            Role = role;
            this.PreemptExpiryTime = _preemptExpiryTime;
        }

        public InstanceProfileAwsCredentials()
            : this(GetFirstRole()) { }

        /// <summary>
        /// The time before actual expiration to expire the credentials.        
        /// Property cannot be set to a negative TimeSpan.
        /// </summary>
        public TimeSpan PreemptExpiryTime
        {
            get { return _preemptExpiryTime; }
            set
            {
                if (value < TimeSpan.Zero) throw new ArgumentOutOfRangeException("value", "PreemptExpiryTime cannot be negative");
                _preemptExpiryTime = value;
            }
        }


        /// <summary>
        /// Returns an instance of ImmutableCredentials for this instance
        /// </summary>
        /// <returns></returns>
        public Credentials GetCredentials()
        {
            lock (_refreshLock)
            {
                // If credentials are expired, update
                if (ShouldUpdate)
                {
                    _currentState = GenerateNewCredentials();

                    // Check if the new credentials are already expired
                    if (ShouldUpdate)
                    {
                        throw new AmazonClientException("The retrieved credentials have already expired");
                    }

                    // Offset the Expiration by PreemptExpiryTime
                    _currentState.Expiration -= PreemptExpiryTime;

                    if (ShouldUpdate)
                    {
                        // This could happen if the default value of PreemptExpiryTime is
                        // overriden and set too high such that ShouldUpdate returns true.
                        System.Diagnostics.Debug.WriteLine(
                            "The preempt expiry time is set too high: Current time = {0}, Credentials expiry time = {1}, Preempt expiry time = {2}.",
                            DateTime.Now,
                            _currentState.Expiration,
                            PreemptExpiryTime);
                    }
                }

                return _currentState.Credentials;
            }
        }

        // Test credentials existence and expiration time
        private bool ShouldUpdate
        {
            get
            {
                return
                    (                                                   // should update if:
                        _currentState == null ||                        //  credentials have not been loaded yet
                        DateTime.Now >= this._currentState.Expiration   //  past the expiration time
                        );
            }
        }



        public static IEnumerable<string> GetAvailableRoles()
        {
            var allAliases = GetContents(RolesUri);
            if (string.IsNullOrEmpty(allAliases))
            {
                yield break;
            }

            var parts = allAliases.Split(AliasSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                var trim = part.Trim();
                if (!string.IsNullOrEmpty(trim))
                {
                    yield return trim;
                }
            }
        }

        
        private CredentialsRefreshState GetEarlyRefreshState(CredentialsRefreshState state)
        {
            // New expiry time = Now + _refreshAttemptPeriod + PreemptExpiryTime
            var newExpiryTime = DateTime.Now + RefreshAttemptPeriod + PreemptExpiryTime;
            // Use this only if the time is earlier than the default expiration time
            if (newExpiryTime > state.Expiration)
                newExpiryTime = state.Expiration;

            return new CredentialsRefreshState
            {
                Credentials = state.Credentials,
                Expiration = newExpiryTime
            };
        }

        private CredentialsRefreshState GetRefreshState()
        {
            SecurityInfo info = GetServiceInfo();
            if (!string.IsNullOrEmpty(info.Message))
            {
                throw new AmazonServiceException(string.Format(CultureInfo.InvariantCulture,
                    "Unable to retrieve credentials. Message = \"{0}\".",
                    info.Message));
            }
            SecurityCredentials credentials = GetRoleCredentials();

            CredentialsRefreshState refreshState = new CredentialsRefreshState
            {
                Credentials = new Credentials(credentials.AccessKeyId, credentials.SecretAccessKey, credentials.Token),
                Expiration = credentials.Expiration
            };

            return refreshState;
        }

        private static SecurityInfo GetServiceInfo()
        {
            string json = GetContents(InfoUri);
            SecurityInfo info = JsonMapper.ToObject<SecurityInfo>(json);
            ValidateResponse(info);
            return info;
        }

        private SecurityCredentials GetRoleCredentials()
        {
            string json = GetContents(CurrentRoleUri);
            SecurityCredentials credentials = JsonMapper.ToObject<SecurityCredentials>(json);
            ValidateResponse(credentials);
            return credentials;
        }

        private static void ValidateResponse(SecurityBase response)
        {
            if (!string.Equals(response.Code, SuccessCode, StringComparison.OrdinalIgnoreCase))
            {
                throw new AmazonServiceException(string.Format(CultureInfo.InvariantCulture,
                    "Unable to retrieve credentials. Code = \"{0}\". Message = \"{1}\".",
                    response.Code, response.Message));
            }
        }

        private static string GetContents(Uri uri)
        {
            try
            {
                var request = HttpWebRequest.Create(uri) as HttpWebRequest;
                var asyncResult = request.BeginGetResponse(null, null);
                using (var response = request.EndGetResponse(asyncResult) as HttpWebResponse)
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException)
            {
                throw new AmazonServiceException("Unable to reach credentials server");
            }
        }

        private static string GetFirstRole()
        {
            var roles = GetAvailableRoles();
            foreach (string role in roles)
            {
                return role;
            }

            // no roles found
            throw new InvalidOperationException("No roles found");
        }
        
        
        private class SecurityBase
        {
            public string Code { get; set; }
            public string Message { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        private class SecurityInfo : SecurityBase
        {
            public string InstanceProfileArn { get; set; }
            public string InstanceProfileId { get; set; }
        }

        private class SecurityCredentials : SecurityBase
        {
            public string Type { get; set; }
            public string AccessKeyId { get; set; }
            public string SecretAccessKey { get; set; }
            public string Token { get; set; }
            public DateTime Expiration { get; set; }
        }

    }
}