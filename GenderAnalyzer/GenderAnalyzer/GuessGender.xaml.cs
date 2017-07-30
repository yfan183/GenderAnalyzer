using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;

using Xamarin.Forms;
using GenderAnalyzer.Model;
using Xamarin.Forms.Xaml;

namespace GenderAnalyzer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GuessGender : ContentPage
    {
        
        public GuessGender()
        {
            InitializeComponent();
        }

        private async void loadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });


            await MakePredictionRequest(file);
        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task MakePredictionRequest(MediaFile file)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", "c318d58e642d4da0a94252bbbd87a76e");

            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/ceeb2fc1-b383-407e-88cb-f1ec50db4054/image?iterationId=db0244c3-7eeb-4ba9-80b0-4ea7ff96df89";

            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);


                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    EvaluationModel responseModel = JsonConvert.DeserializeObject<EvaluationModel>(responseString);

                    double max = responseModel.Predictions.Max(m => m.Probability);
                    foreach (var item in responseModel.Predictions)
                    {
                        if (max <= 0.5)
                        {
                            await DisplayAlert("Wow","Hmm, I can't really tell.","Ok");
                            break;
                        }
                        else if (item.Tag == "male" && item.Probability > 0.7)
                        {
                            await DisplayAlert("Wow", "Hmm, I think this is a male.", "Ok");
                            break;
                        }
                        else if (item.Tag == "female" && item.Probability > 0.7)
                        {
                            await DisplayAlert("Wow", "Hmm, I think this is a female.", "Ok");
                            break;
                        }
                    }
                    
                    

                }

                //Get rid of file once we have finished using it
                file.Dispose();
            }
        }
    }
}