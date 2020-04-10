# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).


## [0.3.0] - 2020-04-10

### Change
- Use 'FaceDataServer/Inject Required Layer & Parameters' menu item to inject required layer and parameters

### Removed
- InjectAnimationLayer in ApplyFaceDataToVRM.cs


## [0.2.0] - 2020-04-07

### Changed
- Use 'Animation' to represents face rotations
- Split Unity related codes into 'FaceDataServer-Unity' classlib


## [0.1.0] - 2020-03-25

### Added
- 'FaceDataServer' class for basement
- 'FaceDataServerReceiver' class for receiving data
- 'ApplyFaceDataToVRM' class to apply FaceData to VRM model on Unity
- 'FaceDataServerComponent' class to start up receiver on Unity
- 'FaceData' class to represent and manipulate FaceData
- Test for 'FaceData' (Though `dotnet test` doesn't work as codes inside 'Unity' dir lacks dependencies)

