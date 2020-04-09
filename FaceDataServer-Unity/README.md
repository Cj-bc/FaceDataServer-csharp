日本語: [JA_README.md](JA_README.md)

---

# FaceDataServer-Unity

For unity scripts and assets.
Currently, VRM is only supported.

# Using with Unity

Describe how to apply data to VRM.

## 1. Setup

1. Grab Unitypackage from Release.
2. Attach [FaceDataServerComponent.cs](FaceDataServerComponent.cs) to object.
  - Don't care which object to attach. I attach it to `FDS` empty object.
3. Attach [ApplyFaceDataToVRM.cs](ApplyFaceDataToVRM.cs) to __VRM object's root__.
4. Modify `AnimatorController` attached to the VRM root with 'FaceDataServer/Inject Required Layer & Paramertes' MenuItem.
   This will automatically set layer and Animation parameters that we need.

## 2. Runtime

[FaceDataServerComponent.cs](FaceDataServerComponent.cs) will automatically receive data from backend.  
[ApplyFaceDataToVRM.cs](ApplyFaceDataToVRM.cs) will automatically apply the data to model.
Nothing special to do here.



