using System.IO;
using Sep.Git.Tfs.VsCommon;
using StructureMap;

namespace Sep.Git.Tfs.Vs2012
{
    public class TfsHelper : TfsHelperVs2012Base
    {
        protected override string TfsVersionString { get { return "11.0"; } }

        public TfsHelper(TextWriter stdout, TfsApiBridge bridge, IContainer container)
            : base(stdout, bridge, container) { }

        protected override string UserExtensionsLocation
        {
            get { return @"AppData\Local\Microsoft\VisualStudio\11.0\Extensions"; }
        }

        protected override string RegistryCheckinPolicies64KeyName
        {
            get { return @"SOFTWARE\Wow6432Node\Microsoft\VisualStudio\11.0\TeamFoundation\SourceControl\Checkin Policies"; }
        }

        protected override string RegistryCheckinPoliciesKeyName
        {
            get { return @"SOFTWARE\Microsoft\VisualStudio\11.0\TeamFoundation\SourceControl\Checkin Policies"; }
        }
    }
}
