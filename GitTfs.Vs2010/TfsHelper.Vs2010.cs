using System;
using System.IO;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;
using Sep.Git.Tfs.VsCommon;
using StructureMap;
using Sep.Git.Tfs.Core.TfsInterop;

namespace Sep.Git.Tfs.Vs2010
{
    public class TfsHelper : TfsHelperBase
    {
        public TfsHelper(TextWriter stdout, TfsApiBridge bridge, IContainer container) : base(stdout, bridge, container)
        {
        }

        private string vsInstallDir;
        protected override string GetVsInstallDir()
        {
            if (vsInstallDir == null)
            {
                vsInstallDir = TryGetRegString(@"Software\Microsoft\VisualStudio\10.0", "InstallDir")
                    ?? TryGetRegString(@"Software\WOW6432Node\Microsoft\VisualStudio\10.0", "InstallDir");
            }
            return vsInstallDir;
        }

        private IGroupSecurityService GroupSecurityService
        {
            get { return GetService<IGroupSecurityService>(); }
        }

        public override IIdentity GetIdentity(string username)
        {
            return _bridge.Wrap<WrapperForIdentity, Identity>(Retry.Do(() => GroupSecurityService.ReadIdentity(SearchFactor.AccountName, username, QueryMembership.None)));
        }

        protected override TfsTeamProjectCollection GetTfsCredential(Uri uri)
        {
            return HasCredentials ?
                    new TfsTeamProjectCollection(uri, GetCredential(), new UICredentialsProvider()) :
                    new TfsTeamProjectCollection(uri, new UICredentialsProvider());
        }

        protected override string UserExtensionsLocation
        {
            get { return @"AppData\Local\Microsoft\VisualStudio\10.0\Extensions"; }
        }

        protected override string RegistryCheckinPolicies64KeyName
        {
            get { return @"SOFTWARE\Wow6432Node\Microsoft\VisualStudio\10.0\TeamFoundation\SourceControl\Checkin Policies"; }
        }

        protected override string RegistryCheckinPoliciesKeyName
        {
            get { return @"SOFTWARE\Microsoft\VisualStudio\10.0\TeamFoundation\SourceControl\Checkin Policies"; }
        }
    }
}
