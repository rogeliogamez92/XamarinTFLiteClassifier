# XamarinTFLiteClassifier

Example of the TensorFlow Lite library (1.9.0) https://www.nuget.org/packages/Xamarin.TensorFlow.Lite/ on Xamarin.Android.


I upload this because I couldn't find an example of TFLite on Xamarin.Android. 

NOTES:
- The classifier's input is a float[] and output is a float[1][]. I just added a Dictionary to have a prettify the output. 
- The MainActivity loads 3 files in this example that are NOT included: Model.tflite (converted from a keras model in my case), Labels.txt (the labels of the classes of the model each in one line), and a Test.csv (data to classify).
- To load Model.tflite as an asset you need to add the .tflite extension to AndroidStoreUncompressedFileExtensions in your .csproj.

```
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="4.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    ...
    <AndroidStoreUncompressedFileExtensions>tflite</AndroidStoreUncompressedFileExtensions>
  </PropertyGroup>
  ....
</Project>
```

For more information visit https://www.tensorflow.org/lite/, https://codelabs.developers.google.com/codelabs/tensorflow-for-poets-2-tflite/ or create an issue and I'll try to respond it.
