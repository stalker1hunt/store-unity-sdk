using System.IO;
using UnityEditor.Android;
using UnityEngine;

namespace Scenes.PaymentUnitySDK.Editor
{
    public class GradlePostProcessor : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder => 999;

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            string gradlePath = Path.Combine(path, "build.gradle");

            if (File.Exists(gradlePath))
            {
                string gradleContent = File.ReadAllText(gradlePath);

                string dependenciesToAdd = "\n  implementation 'androidx.multidex:multidex:2.0.1'\n" +
                                           "    implementation 'com.google.android.gms:play-services-wallet:19.3.0'\n" +
                                           "    implementation 'com.google.android.gms:play-services-auth:21.1.1'\n" +
                                           "    implementation 'androidx.core:core-ktx:1.13.1'\n" +
                                           "    implementation 'androidx.appcompat:appcompat:1.6.1'\n" +
                                           "    implementation 'com.google.android.material:material:1.12.0'\n" +
                                           "    implementation 'androidx.constraintlayout:constraintlayout:2.1.4'\n";
                if (!gradleContent.Contains(dependenciesToAdd))
                {
                    gradleContent = gradleContent.Replace("dependencies {", "dependencies {" + dependenciesToAdd);
                }

                string repoToAdd = "google()\n        jcenter()";
                if (!gradleContent.Contains(repoToAdd))
                {
                    gradleContent = gradleContent.Replace("repositories {", "repositories {\n        " + repoToAdd);
                }

                File.WriteAllText(gradlePath, gradleContent);
            }
            else
            {
                Debug.LogError(gradlePath);
            }

            var manifestPath = Path.Combine(path, "src/main/AndroidManifest.xml");

            if (File.Exists(manifestPath))
            {
                var manifest = File.ReadAllText(manifestPath);

                if (!manifest.Contains("<uses-permission android:name=\"android.permission.INTERNET\" />"))
                {
                    manifest = manifest.Replace("<application",
                        "<uses-permission android:name=\"android.permission.INTERNET\" />\n<application");
                    File.WriteAllText(manifestPath, manifest);
                }
            }
            else
            {
                Debug.LogError(manifestPath);
            }
        }
    }
}