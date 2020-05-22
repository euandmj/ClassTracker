### ClassTracker
a mini lightweight experimental project. Aims to provide a easy to use framework to be aware of whats changed in a class.

## Build Status
  ![.NET Core](https://github.com/euandmj/ClassTracker/workflows/.NET%20Core/badge.svg?branch=master&event=push)
  [![NuGet](https://img.shields.io/nuget/v/ClassTracker.svg?style=flat)](https://www.nuget.org/packages/ClassTracker/)

## Usage
  See the [Examples](https://github.com/euandmj/ClassTracker/tree/master/Examples) project for more details
  
### Mark your object members with the TrackedItem attribute.
##### Applies to mutable public/private/protected/internal Properties/Fields

        class Apple
        {
            // Tracked property
            [TrackedItem]
            public Color Color { get; set; }
            
            // Tracked field
            [TrackedItem]
            public int Weight;
            
            // Non-Tracked field
            public Size Size;
        }

### Register an instance of an object with 
##### A MemberInfoException will be thrown for non readonly/const members
    
        ClassTracker<T>.Register(T obj)

### Assign the changed values of A to B

        ClassTracker<T>.AssignTo(T a, T b)
### Assign the tracked values from A to B irrelevant of their changed status

        ClassTracker<T>.BlindAssignTo(T a, T b)
### Reset an instance to the values that are Recorded
        ClassTracker<T>.ResetDefaults(T obj)


