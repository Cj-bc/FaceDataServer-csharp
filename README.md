# FaceDataServer-csharp

This is implementation of [FaceDataServer protocol](https://github.com/Cj-bc/FDS-protos) for C#.

# Usage

## Using with Unity

1. Grab Unitypackage from Release.
2. Attach [FaceDataServerComponent.cs](FaceDataServer/Unity/FaceDataServerComponent.cs) to object.
  - Don't care which object to attach. I attach it to `FDS` empty object.
3. Attach [ApplyFaceDataToVRM.cs](FaceDataServer/Unity/ApplyFaceDataToVRM.cs) to __VRM object's root__.
4. Make sure VRM's _Animator_ holds AnimatorController.


## Using as nuget package
