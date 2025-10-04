# Unity Process Action Catalogue

This guide outlines newly added process actions tailored for common Unity game development workflows and captures a backlog of additional ideas you can explore next. Each action follows the `ProcessAction` pattern already present in the project, allowing them to be dropped into existing Jungle pipelines with minimal integration work.

## Newly Added Actions

| Action | Category | What it does |
| --- | --- | --- |
| `PositionLerpAction` | Transform | Tweens a transform to a target world or local position with curve-driven easing and optional return when stopped. |
| `RotationLerpAction` | Transform | Smoothly rotates a transform toward target Euler angles with support for local or world space and reversible playback. |
| `AnimatorTriggerAction` | Animation | Fires a configured Animator trigger on start and optionally resets it on stop so one-off animations can be orchestrated safely. |
| `AnimatorFloatLerpAction` | Animation | Gradually adjusts a float parameter on an Animator controller, ideal for blending layers, weight transitions, or procedural effects. |

## Backlog of Suggested Process Actions

The following ideas cover a broad spectrum of gameplay and presentation needs. They can be implemented incrementally, following the patterns shown in the new actions above.

### Transform & Motion
- **PathFollowAction** – Move a transform along a predefined spline or waypoint list with speed controls and looping.
- **ScalePulseAction** – Continuously pulse an object's scale between two ranges for highlight effects.
- **TransformShakeAction** – Apply Perlin-noise-based shaking for camera hits or item pickups with adjustable intensity curves.
- **AnchorSwapAction** – Transition UI elements between anchors or layouts while preserving perceived motion.

### Animation & Rigging
- **AnimatorBoolLatchAction** – Toggle animator bool parameters with automatic reset rules.
- **AnimatorLayerWeightAction** – Blend animator layer weights over time for layered animation systems.
- **AvatarMaskBlendAction** – Swap avatar masks or override controllers and fade them in via layer weights.
- **TimelineControlAction** – Start, pause, and resume Timeline PlayableDirector instances through process orchestration.

### VFX & Audio
- **ParticleBurstAction** – Trigger particle systems with configurable burst patterns and optional cleanup on stop.
- **MaterialEmissionPulseAction** – Animate material emission values for warning lights or spell charging cues.
- **AudioSnapshotBlendAction** – Crossfade between audio mixer snapshots using durations defined per transition.
- **DecalFadeAction** – Fade projector or decal materials in and out to mark impacts or area effects.

### Gameplay Systems
- **PhysicsConstraintAction** – Enable or disable rigidbody constraints or joint properties during scripted sequences.
- **NavMeshAgentMoveAction** – Command a NavMeshAgent toward a destination and optionally wait for arrival before completing.
- **ColliderToggleAction** – Toggle collider enable states with deferred reset, useful for interaction windows.
- **InventoryItemSpawnAction** – Instantiate item prefabs with pooling hooks and signal completion once spawned.

### UI & Interaction
- **CanvasGroupFadeAction** – Fade UI groups using CanvasGroup alpha, optionally blocking interaction until complete.
- **InputStateAction** – Enable or disable input maps or action sets to gate player control during cutscenes.
- **TooltipDisplayAction** – Show contextual tooltips with timers and animation hooks.
- **CursorStyleAction** – Swap cursor sprites or lock states when hovering over interactive sequences.

### Camera & Presentation
- **CameraBlendAction** – Blend between Cinemachine virtual cameras or modify blend curves dynamically.
- **FieldOfViewLerpAction** – Animate camera FOV for dash effects or aim-down-sights transitions.
- **ColorGradingAction** – Blend post-processing volumes or override weights for dramatic lighting shifts.
- **LetterboxAction** – Animate screen safe-area bars in/out for cinematic sequences.

### Systems Integration
- **SaveGameSnapshotAction** – Capture or restore checkpoints by triggering the save system from process sequences.
- **NetworkRPCAction** – Invoke networked RPC calls within multiplayer contexts while ensuring proper callbacks.
- **AnalyticsEventAction** – Dispatch analytics events tied to cinematic beats or tutorial completions.
- **StateMachineAction** – Interface with custom state machines, pushing/popping states as part of a process.

Each suggestion can build on the robust coroutine-driven infrastructure established here. Aim to include validation that surfaces configuration issues early (for example, missing components) to keep authoring experiences smooth inside the editor.
