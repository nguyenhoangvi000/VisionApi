using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Google.Apis;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;

namespace VisionAPI
{
    public class FaceDetectApp
    {

        private VisionService _vision;
        

        public FaceDetectApp(VisionService vision)
        {
            this._vision = vision;
        }
        public IList<FaceAnnotation> detectFaces(string path, int maxResults)
        {
            byte[] data = File.ReadAllBytes(path);

            AnnotateImageRequest request = new AnnotateImageRequest();
            Google.Apis.Vision.v1.Data.Image img = new Google.Apis.Vision.v1.Data.Image();
            img.Content = Convert.ToBase64String(data);
            request.Image = img;

            Feature feature = new Feature();
            feature.Type = "FACE_DETECTION";
            feature.MaxResults = maxResults;

            request.Features = new List<Feature>()
            {
                feature
            };
            


            BatchAnnotateImagesRequest batchAnnotate = new BatchAnnotateImagesRequest();
            batchAnnotate.Requests = new List<AnnotateImageRequest>() {
                request
            };
            ImagesResource.AnnotateRequest annotate = _vision.Images.Annotate(batchAnnotate);

            BatchAnnotateImagesResponse batchResponse = annotate.Execute();

            AnnotateImageResponse response = batchResponse.Responses[0];

            if (response.FaceAnnotations == null)
            {
                throw new Exception(response.Error.Message);
            }

            return response.FaceAnnotations;

        }

        

        public void annotateWithFaces(Bitmap bmp, IList<FaceAnnotation> faces)
        {
            foreach (FaceAnnotation face in faces)
            {
                annotateWithFace(bmp, face);
            }
            
        }



        private void annotateWithFace(Bitmap bmp, FaceAnnotation face)
        {
            Graphics graphics = Graphics.FromImage(bmp);
            List<Point> point = new List<Point>();

            foreach (var item in face.BoundingPoly.Vertices)
            {
                
                point.Add(new Point(item.X.Value, item.Y.Value));

            }

            point.Add(point[0]);
            graphics.DrawLines(Pens.Aqua, point.ToArray());

            graphics.Dispose();
        }


       

    }
}
