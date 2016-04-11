using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using VisionAPI;
using Google.Apis.Vision.v1;
using Google.Apis.Services;
using Google.Apis.Vision.v1.Data;

namespace SmartHome
{
    public static class Program
    {
        private static string Application_Name = "Face Detect";
        private static int MAX_RESULTS = 20;

        public static void Main()
        {
            FaceDetectApp app = new FaceDetectApp(CreateVisionServiceClient());

            var faces = app.detectFaces("E:\\2.jpg", MAX_RESULTS);

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap("E:\\2.jpg");

            app.annotateWithFaces(bmp, faces);
            bmp.Save("E:\\21.jpg");
        }

        public static VisionService CreateVisionServiceClient()
        {
            using (StreamReader sr=new StreamReader("E:\\app.json"))
            {
                var credentials = Google.Apis.Auth.OAuth2.GoogleCredential.FromStream(sr.BaseStream);


                if (credentials.IsCreateScopedRequired)
                {
                    credentials = credentials.CreateScoped(new[] { VisionService.Scope.CloudPlatform });
                }

                var serviceInitializer = new BaseClientService.Initializer()
                {
                    ApplicationName = Application_Name,
                    HttpClientInitializer = credentials
                };
                return new VisionService(serviceInitializer);
            }
        }
    }
}