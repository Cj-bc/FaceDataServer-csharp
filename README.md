日本語: [JA_README.md](JA_README.md)

---

# FaceDataServer-csharp

This is implementation of [FaceDataServer protocol](https://github.com/Cj-bc/FDS-protos) for C#.

# what is FaceDataServer?

__FaceDataServer__ is a project to do face tracking with various frontends(i.e. 3D model, Live2D, ASCII Art)  
For details, please refer to [FaceDataServer protocol](https://github.com/Cj-bc/FDS-protos).

# Usage

## Using with Unity

1. Grab Unitypackage from Release.
2. Attach [FaceDataServerComponent.cs](FaceDataServer-Unity/FaceDataServerComponent.cs) to object.
  - Don't care which object to attach. I attach it to `FDS` empty object.
3. Attach [ApplyFaceDataToVRM.cs](FaceDataServer-Unity/ApplyFaceDataToVRM.cs) to __VRM object's root__.
4. Make sure VRM's _Animator_ holds AnimatorController.


## Using as nuget package

Not yet
