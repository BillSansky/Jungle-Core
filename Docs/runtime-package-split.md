# Runtime package split roadmap

## Core runtime packages
- `jungle.core` delivers the shared abstractions that every module relies on, including the action interfaces, process helpers,
  timing utilities, and the mandatory value infrastructure (such as `IValue<T>`, `LocalValue<T>`, and the ScriptableObject/
  component bases).
- `Packages/Jungle.Values` keeps the full catalogue of runtime value providers—primitive types, Unity structs, and gameplay
  component lookups—bundled together so consumers can install one package and gain every default implementation.

These two packages represent the minimal footprint a plugin must depend on to integrate with Jungle runtimes.

## Optional runtime packages
- `Packages/Jungle.Actions` now organizes its implementations under `Runtime/Action/Implementations/ThreeD` for animation,
  object, physics, and graphics/audio behaviours, and under `Runtime/Action/Implementations/TwoD` for sprite-specific rendering
  tweaks. Teams can lift one or both directories into dedicated packages if they want to redistribute the actions separately.
- `Packages/Jungle.State` keeps the save-state helpers and default Transform/Rigidbody snapshots so projects can opt into
  persistence tooling as needed.
- `Packages/Jungle.Events` hosts the ScriptableObject event assets, listener behaviours, and callback implementations that sit
  on top of the lightweight core contracts.
- `Packages/Jungle.Utils` remains an optional toolbox that offers runtime singletons and convenience behaviours that higher
  level packages can adopt when they need them.

## Migration notes
1. Each package publishes its own assembly definition and manifest, declaring explicit references only to the runtime modules it
   consumes. Optional packages should depend on the relevant core modules (for example, both action packages reference the core
   action and value assemblies while the state package leans on the value system).
2. When extracting additional packages, move both the C# files and their `.meta` companions to keep GUID references stable across
   Unity projects.
3. Publish each runtime package independently so downstream projects can install only the functionality they need while sharing
   the foundational interfaces from `jungle.core`.
