// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using AccessibilityInsights.Extensions.AzureDevOps;
using AccessibilityInsights.Extensions.AzureDevOps.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AccessibilityInsights.Extensions.AzureDevOpsTests
{
    [TestClass]
    public class AzureDevOpsIntegrationTests
    {
        private const string FeatureName = "FeatureName";  // Defined in the TelemetryHint class, but we don't want coupling
        private const string DefaultFeatureName = "NoFeatureMapped";
        private const string AreaName = "AccessibilityInsights";

        [TestMethod]
        [Timeout(1000)]
        public void TestGetTeamProjectUrl()
        {
            using (ShimsContext.Create())
            {
                ShimAzureDevOpsIntegration.AllInstances.ConnectedToAzureDevOpsGet = (_) => true;
                ShimAzureDevOpsIntegration.AllInstances.ConnectedUriGet = (_) => new Uri("https://myaccount.visualstudio.com");
                Assert.AreEqual("https://myaccount.visualstudio.com/my%20project/my%20team", AzureDevOpsIntegration.GetCurrentInstance().GetTeamProjectUrl("my project", "my team").AbsoluteUri);

                ShimAzureDevOpsIntegration.AllInstances.ConnectedUriGet = (_) => new Uri("https://myaccount.visualstudio.com");
                Assert.AreEqual("https://myaccount.visualstudio.com/my%20project/my%20team", AzureDevOpsIntegration.GetCurrentInstance().GetTeamProjectUrl("my project", "my team").AbsoluteUri);

                // trailing slash
                ShimAzureDevOpsIntegration.AllInstances.ConnectedUriGet = (_) => new Uri("https://myaccount.visualstudio.com/");
                Assert.AreEqual("https://myaccount.visualstudio.com/my%20project/my%20team", AzureDevOpsIntegration.GetCurrentInstance().GetTeamProjectUrl("my project", "my team").AbsoluteUri);
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void TestGetTeamProjectUrl_SpaceInArgument_Encoded()
        {
            using (ShimsContext.Create())
            {
                ShimAzureDevOpsIntegration.AllInstances.ConnectedToAzureDevOpsGet = (_) => true;
                ShimAzureDevOpsIntegration.AllInstances.ConnectedUriGet = (_) => new Uri("https://myaccount.visualstudio.com");
                Assert.AreEqual("https://myaccount.visualstudio.com/my%20project/my%20team", AzureDevOpsIntegration.GetCurrentInstance().GetTeamProjectUrl("my project", "my team").AbsoluteUri);

                // trailing slash
                ShimAzureDevOpsIntegration.AllInstances.ConnectedUriGet = (_) => new Uri("https://myaccount.visualstudio.com/");
                Assert.AreEqual("https://myaccount.visualstudio.com/my%20project/my%20team", AzureDevOpsIntegration.GetCurrentInstance().GetTeamProjectUrl("my project", "my team").AbsoluteUri);
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void TestGetTeamProjectUrl_NullTeam()
        {
            using (ShimsContext.Create())
            {
                ShimAzureDevOpsIntegration.AllInstances.ConnectedToAzureDevOpsGet = (_) => true;

                ShimAzureDevOpsIntegration.AllInstances.ConnectedUriGet = (_) => new Uri("https://myaccount.visualstudio.com");
                Assert.AreEqual("https://myaccount.visualstudio.com/myproject", AzureDevOpsIntegration.GetCurrentInstance().GetTeamProjectUrl("myproject", null).AbsoluteUri);

                ShimAzureDevOpsIntegration.AllInstances.ConnectedUriGet = (_) => new Uri("https://myaccount.visualstudio.com/");
                Assert.AreEqual("https://myaccount.visualstudio.com/myproject", AzureDevOpsIntegration.GetCurrentInstance().GetTeamProjectUrl("myproject", null).AbsoluteUri);

                ShimAzureDevOpsIntegration.AllInstances.ConnectedUriGet = (_) => new Uri("https://myaccount.visualstudio.com/");
                Assert.AreEqual("https://myaccount.visualstudio.com/my%20project", AzureDevOpsIntegration.GetCurrentInstance().GetTeamProjectUrl("my project", null).AbsoluteUri);
            }
        }
    }
    
}
