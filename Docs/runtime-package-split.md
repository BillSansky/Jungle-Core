# Runtime package split roadmap

## Current runtime packages
- `Packages/Jungle.Core` now contains the shared abstractions that other runtime modules depend on, including the action
  interfaces, event monitor contracts, and shared timing utilities.【F:Packages/Jungle.Core/Runtime/Action/IProcessAction.cs†L1-L48】
【F:Runtime/Event/IEventMonitor.cs†L1-L33】【F:Packages/Jungle.Core/Runtime/Timing/Timer.cs†L1-L150】
- `Packages/Jungle.Actions` houses all built-in action behaviours and references only the foundational core and utility
  assemblies that provide coroutine infrastructure.【F:Packages/Jungle.Actions/Runtime/Action/Implementations/Immediate Actions/ImmediateStateAction.cs†L1-L71】
【F:Packages/Jungle.Actions/Runtime/Jungle.Actions.asmdef†L1-L41】
- `Packages/Jungle.Values` ships both the value system and the condition graph so features that bind values can pull in the
  predicates they require without installing another package.【F:Packages/Jungle.Values/Runtime/Condition/Condition.cs†L1-L39】
【F:Packages/Jungle.Values/Runtime/Jungle.Values.asmdef†L1-L40】
- `Packages/Jungle.Events` hosts ScriptableObject event assets, MonoBehaviour listeners, and callback implementations that
  sit on top of the lightweight core contracts while leaning on the values and utility packages.【F:Packages/Jungle.Events/Runtime/Event/CallbackBehaviour.cs†L1-L112】
【F:Packages/Jungle.Events/Runtime/Jungle.Events.asmdef†L1-L42】
- `Packages/Jungle.State` isolates the generic save-state helpers and built-in Transform/Rigidbody state definitions so
  projects can opt into persistence tooling independently.【F:Packages/Jungle.State/Runtime/State/ObjectStateRecorder.cs†L1-L188】
【F:Packages/Jungle.State/Runtime/Jungle.State.asmdef†L1-L38】
- `Packages/Jungle.Utils` offers optional runtime singletons such as the coroutine runner and ScriptableObject
  convenience base class that higher-level packages can opt into when needed.【F:Packages/Jungle.Utils/Runtime/Utils/CoroutineRunner.cs†L1-L72】
【F:Packages/Jungle.Utils/Runtime/Jungle.Utils.asmdef†L1-L38】

## Migration notes
1. Each package publishes its own assembly definition and manifest, declaring explicit references only to the runtime modules it
   consumes (for example, `Jungle.Events` links to the values and utils assemblies while `Jungle.Actions` only depends on core
   and utils).【F:Packages/Jungle.Events/Runtime/Jungle.Events.asmdef†L1-L42】【F:Packages/Jungle.Actions/Runtime/Jungle.Actions.asmdef†L1-L41】
2. When extracting additional packages, move both the C# files and their `.meta` companions to keep GUID references stable
   across Unity projects.
3. Publish each runtime package independently so downstream projects can install only the functionality they need while
   sharing the foundational interfaces from `jungle.core`.【F:Packages/Jungle.Core/package.json†L1-L18】【F:Packages/Jungle.Events/package.json†L1-L22】
