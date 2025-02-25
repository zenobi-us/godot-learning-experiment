# Godot RPG Project

## Setup

1. Clone the repository
2. Install Godot Engine: v4.3.stable.mono.official [77dcf97d8]
3. `sudo dnf install dotnet-sdk-7.0-7.0.410-1.x86_64` (todo: use mise/asdf for this)
4. open project from terminal: `Godot_v4.3-stable_mono_linux_x86_64/Godot_v4.3-stable_mono_linux.x86_64 --editor`

## 

To use this in Godot:

    Create a new Godot project with C# support enabled
    Create these classes in separate files in your project
    Attach the Game script to your main scene node
    Make sure your project is set up with Mono (Godot's C# integration)

This ECS implementation includes:

    Entity: A simple ID container
    Components: Data containers (Position and Velocity as examples)
    Systems: Logic processors (MovementSystem as an example)
    ECS Manager: Handles entity creation and system processing

Key features:

    Type-safe component system
    Automatic system processing based on required components
    Integrated with Godot's node system
    Frame-based processing using Godot's _Process method

https://grok.com/chat/b99b7021-4a55-4ed6-80f7-d0e6f999e6fc
