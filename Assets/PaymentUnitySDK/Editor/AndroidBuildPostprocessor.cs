using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Scenes.PaymentUnitySDK.Editor
{
    public class AndroidBuildPostprocessor : IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 1; } }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.Android)
            {
                string gradlePropertiesPath = Path.Combine(Application.dataPath, "Plugins/Android/gradle.properties");
                File.AppendAllText(gradlePropertiesPath, "\nandroid.useAndroidX=true\nandroid.enableJetifier=true\n");

                string gradleFilePath = Path.Combine(Application.dataPath, "Plugins/Android/mainTemplate.gradle");
                string gradleContent = File.ReadAllText(gradleFilePath);
                gradleContent = gradleContent.Replace("minifyEnabled false", "minifyEnabled true");
                File.WriteAllText(gradleFilePath, gradleContent);
            }
        }
    }
}