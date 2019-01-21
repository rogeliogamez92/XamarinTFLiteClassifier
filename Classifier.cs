using System;
using System.Collections.Generic;
using Xamarin.TensorFlow.Lite;
using Java.Util;

namespace XamarinTFLite
{
    public class Classifier : IDisposable
    {

        public Interpreter Interpreter { get; set; }
        public List<string> Labels { get; set; }

        public Classifier()
        {

        }

        public Result<Dictionary<string, float>> Classify(float[] inputArray)
        {
            var result = new Result<Dictionary<string, float>>()
            {
                Data = new Dictionary<string, float>()
            };

            try
            {
                var outputArray = new float[1][] { new float[Labels.Count] };
                var outputArrayJavaObj = Arrays.FromArray(outputArray);
             
                Interpreter.Run(inputArray, outputArrayJavaObj);

                outputArray = outputArrayJavaObj.ToArray<float[]>();

                for (var i = 0; i < Labels.Count; i++)
                {
                    result.Data.Add(Labels[i], outputArray[0][i]);
                }
            }
            catch (Exception ex)
            {
                result.ResolveException(ex);
            }

            return result;
        }

        public void Dispose()
        {
            if (Interpreter != null)
            {
                Interpreter.Close();
                Interpreter = null;
            }
        }
    }

    public class Result
    {

        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public Exception Exception { get; set; }

        public void ResolveException(Exception ex)
        {
            Success = false;
            Message = ex.Message;
            Exception = ex;
        }

    }

    public class Result<T> : Result
    {

        public T Data { get; set; }

    }

}
