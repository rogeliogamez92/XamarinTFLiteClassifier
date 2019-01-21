using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Xamarin.TensorFlow.Lite;
using Java.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using Android.Content;

namespace XamarinTFLite
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        Classifier classifier;
        List<string> labels;
        List<float[]> data;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_activity);

            classifier = new Classifier();
            LoadModel();
            LoadLabels();
            LoadData();
            Test();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (classifier != null)
            {
                classifier.Dispose();
            }
        }

        public void LoadModel()
        {
            var fileDescriptor = Assets.OpenFd("Model.tflite");
            var inputStream = new FileInputStream(fileDescriptor.FileDescriptor);
            var fileChannel = inputStream.Channel;
            var startOffset = fileDescriptor.StartOffset;
            var declaredLength = fileDescriptor.DeclaredLength;
            var modelFile = fileChannel.Map(Java.Nio.Channels.FileChannel.MapMode.ReadOnly, startOffset, declaredLength);

            classifier.Interpreter = new Interpreter(modelFile);
        }

        public void LoadLabels()
        {
            labels = new List<string>();
            using (var file = new System.IO.StreamReader(Assets.Open("Labels.txt")))
            {
                var line = string.Empty;
                while ((line = file.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        labels.Add(line);
                    }
                }
            }

            classifier.Labels = labels;
        }

        public void LoadData()
        {
            data = new List<float[]>();
            using (var file = new System.IO.StreamReader(Assets.Open("Test.csv")))
            {
                var line = string.Empty;
                while ((line = file.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var splitted = line.Split(',').ToList();
                        var array = splitted.Select(s => float.Parse(s)).ToArray();
                        data.Add(array);
                    }
                }
            }
        }

        public void Test()
        {
            foreach (var data in data)
            {
                var result = data.Last();
                data[data.Length - 1] = 0;
                var classify = classifier.Classify(data.ToArray());

                if (classify.Success)
                {
                    float prediction = classify.Data.ElementAt(0).Value;
                    var msg = string.Format("Result:{0:0.000000} Prediction:{1:0.000000}", result, prediction);
                }
            }
        }

    }
}
