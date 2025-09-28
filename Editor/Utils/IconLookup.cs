#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Jungle.Editor
{
    /// <summary>
    /// Centralized, quiet icon resolver for Jungle tools.
    /// Order:
    /// 1) Explicit asset path / key via ForKeyOrPath()
    /// 2) Jungle Core package icons (Packages/com.jungle.core/Editor/Icons)
    /// 3) Editor Default Resources (Assets/Editor Default Resources/Icons)
    /// 4) Built-in editor textures by token
    /// 5) Mini type thumbnail (for UnityEngine.Object)
    /// 6) Script icon
    /// </summary>
    public static class IconLookup
    {
        /// <summary>
        /// Static root for extra Jungle icons inside the Jungle Core package.
        /// Adjust if your package name/path differs.
        /// </summary>
        public static string JungleIconsRoot = "Packages/Jungle Core/Editor/Icons";

        private static readonly Dictionary<string, Texture2D> KeyCache = new();
        private static readonly Dictionary<Type, Texture2D> TypeCache = new();

        /// <summary>Loads an icon by explicit asset path or short key (quiet; no console spam).</summary>
        public static Texture2D ForKeyOrPath(string keyOrPath)
        {
            if (string.IsNullOrEmpty(keyOrPath)) return null;
            if (KeyCache.TryGetValue(keyOrPath, out var cached) && cached) return cached;

            // 1) Explicit asset path (e.g., Assets/... or Packages/...)
            if (keyOrPath.Contains("/") && System.IO.Path.HasExtension(keyOrPath))
            {
                var atPath = AssetDatabase.LoadAssetAtPath<Texture2D>(keyOrPath);
                if (atPath) return KeyCache[keyOrPath] = atPath;
            }

            // 2) Jungle package icons (by filename, no extension needed)
            var fromJungle = LoadFromJungleIcons(keyOrPath);
            if (fromJungle) return KeyCache[keyOrPath] = fromJungle;

            // 3) Editor Default Resources (Assets/Editor Default Resources/Icons/)
            var edr1 = EditorGUIUtility.Load(keyOrPath) as Texture2D; // allow "Icons/Name.png"
            if (edr1) return KeyCache[keyOrPath] = edr1;
            var edr2 = EditorGUIUtility.Load($"Icons/{keyOrPath}.png") as Texture2D; // allow just "Name"
            if (edr2) return KeyCache[keyOrPath] = edr2;

            // 4) Built-in/registered textures (quiet if missing)
            var found = EditorGUIUtility.FindTexture(keyOrPath);
            if (found) return KeyCache[keyOrPath] = (Texture2D)found;

            // Try dark-skin variant, too
            if (!keyOrPath.StartsWith("d_", StringComparison.Ordinal))
            {
                found = EditorGUIUtility.FindTexture("d_" + keyOrPath);
                if (found) return KeyCache[keyOrPath] = (Texture2D)found;
            }

            return KeyCache[keyOrPath] = null;
        }

        /// <summary>Guesses an icon for a type using name+namespace tokens and known editor keys.</summary>
        public static Texture2D ForType(Type t)
        {
            if (t == null) return null;
            if (TypeCache.TryGetValue(t, out var tex) && tex) return tex;

            var tokens = GetTokens(t).ToList();

            // 1) Jungle package icons by token name
            tex = TryTokens(tokens, TryJungleToken);
            if (tex) return TypeCache[t] = tex;

            // 2) Built-in editor icons by category/token
            tex = TryTokens(tokens, TryBuiltinToken);
            if (tex) return TypeCache[t] = tex;

            // 3) Unity thumbnails for engine types
            if (typeof(UnityEngine.Object).IsAssignableFrom(t))
            {
                tex = AssetPreview.GetMiniTypeThumbnail(t);
                if (tex) return TypeCache[t] = tex;
            }

            // 4) Script icon (last resort)
            var script = EditorGUIUtility.FindTexture("cs Script Icon") as Texture2D;
            return TypeCache[t] = script ?? Texture2D.grayTexture;
        }

        // ---------- internals ----------

        private static Texture2D TryTokens(IEnumerable<string> tokens, Func<string, Texture2D> tryOne)
        {
            foreach (var tok in tokens)
            {
                var tex = tryOne(tok);
                if (tex) return tex;
            }
            return null;
        }

        private static Texture2D LoadFromJungleIcons(string nameOrPathNoExt)
        {
            if (string.IsNullOrEmpty(JungleIconsRoot)) return null;

            // Allow callers to pass either "Name", "Icons/Name", or "Icons/Name.png"
            var shortName = nameOrPathNoExt.Replace("\\", "/");
            shortName = shortName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                ? shortName.Substring(0, shortName.Length - 4)
                : shortName;

            // Try a few patterns
            var candidates = new[]
            {
                $"{JungleIconsRoot}/{shortName}.png",
                $"{JungleIconsRoot}/{shortName.ToLowerInvariant()}.png",
                $"{JungleIconsRoot}/icon_{shortName}.png",
                $"{JungleIconsRoot}/{ToPascal(shortName)}.png",
            };
            foreach (var p in candidates)
            {
                var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(p);
                if (tex) return tex;
            }
            return null;
        }

        private static Texture2D TryJungleToken(string token)
        {
            // Try token file directly inside JungleIconsRoot
            return LoadFromJungleIcons(token);
        }

        private static Texture2D TryBuiltinToken(string token)
        {
            foreach (var key in BuiltinKeysFor(token))
            {
                var tex = EditorGUIUtility.FindTexture(key);
                if (tex) return (Texture2D)tex;

                if (!key.StartsWith("d_", StringComparison.Ordinal))
                {
                    var dk = "d_" + key;
                    tex = EditorGUIUtility.FindTexture(dk);
                    if (tex) return (Texture2D)tex;
                }
            }

            // Dynamic variants Unity often uses
            var pascal = ToPascal(token);
            var dynamic = new[]
            {
                $"{pascal} Icon",
                $"{pascal}Asset Icon",
                $"{pascal}Controller Icon",
                $"{pascal}Renderer Icon",
                $"{pascal}Data Icon",
                pascal
            };
            foreach (var k in dynamic)
            {
                var tex = EditorGUIUtility.FindTexture(k);
                if (tex) return (Texture2D)tex;
            }
            return null;
        }

        private static IEnumerable<string> BuiltinKeysFor(string token)
        {
            token = token.ToLowerInvariant();
            switch (token)
            {
                // Rendering / Visual
                case "camera": case "vcam": case "cinemachine": case "Cam":return new[] { "Camera Icon" };
                case "light": case "lamp": case "spot": case "point": return new[] { "Light Icon" };
                case "sprite": case "image": case "icon": case "texture": return new[] { "Texture Icon", "Texture2D Icon", "Sprite Icon", "SpriteAtlasAsset Icon" };
                case "material": case "mat": case "surface": return new[] { "Material Icon" };
                case "shader": case "hlsl": case "shadergraph": return new[] { "Shader Icon", "ShaderGraph Icon" };
                case "mesh": case "model": case "fbx": return new[] { "Mesh Icon", "ModelImporter Icon" };
                case "decal": case "trail": case "line": case "gizmo": return new[] { "TrailRenderer Icon", "LineRenderer Icon" };

                // Lighting / Env
                case "sky": case "skybox": case "fog": case "ambient": return new[] { "LightingDataAsset Icon" };
                case "terrain": return new[] { "Terrain Icon", "TerrainData Icon" };

                // Animation / Characters
                case "animator": case "controller": return new[] { "AnimatorController Icon" };
                case "animation": case "clip": return new[] { "AnimationClip Icon" };
                case "rig": case "avatar": case "bone": case "humanoid": return new[] { "Avatar Icon", "AvatarMask Icon" };
                case "timeline": case "playable":  return new[] { "TimelineAsset Icon", "PlayableDirector Icon" };

                // VFX
                case "particle": case "vfx": case "fx": case "shuriken": return new[] { "ParticleSystem Icon" };

                // Audio
                case "audio": case "sound": case "music": case "sfx": case "voice": return new[] { "AudioSource Icon", "AudioMixerController Icon", "AudioListener Icon" };

                // Physics
                case "rigidbody": case "rb": return new[] { "Rigidbody Icon" };
                case "collider": case "hitbox": case "trigger": case "bounds": return new[] { "BoxCollider Icon", "SphereCollider Icon", "CapsuleCollider Icon", "MeshCollider Icon", "WheelCollider Icon" };
                case "joint": case "hinge": case "spring": case "fixedjoint": return new[] { "HingeJoint Icon", "SpringJoint Icon", "FixedJoint Icon" };
                case "physics": case "physicmaterial": return new[] { "PhysicMaterial Icon" };

                // Navigation / AI
                case "nav": case "navmesh": case "agent": case "path": case "pathfind": case "astar": return new[] { "NavMeshData Icon", "Navigation Icon" };
                case "ai": case "behavior": case "behaviour": case "bt": case "goap": case "state": case "fsm": return new[] { "StateMachine Icon", "AnimatorController Icon", "ScriptableObject Icon" };

                // UI / UX
                case "ui": case "hud": case "canvas": case "eventsystem": case "button": case "text": case "dropdown": case "slider": case "toggle": case "scroll":
                    return new[] { "Canvas Icon", "EventSystem Icon", "UnityEditor.UI Image Icon" };
                case "minimap": case "map": case "compass": case "radar": return new[] { "Sprite Icon", "Texture Icon" };

                // Input
                case "input": case "rewired": case "controls": case "binding": case "gamepad": case "keyboard": case "mouse": return new[] { "Settings Icon", "Prefab Icon" };

                // Net / Multiplayer
                case "net": case "network": case "multiplayer": case "client": case "server": case "lobby": case "match": case "rpc": return new[] { "NetworkManager Icon", "Settings Icon", "Prefab Icon" };

                // Data / Save / Serialization
                case "save": case "load": case "prefs": case "json": case "yaml": case "binary": case "serialize": return new[] { "ScriptableObject Icon", "TextAsset Icon" };
                case "addressable": case "bundle": case "assetref": return new[] { "Prefab Icon", "ScriptableObject Icon" };

                // Gameplay
                case "combat": case "damage": case "health": case "hp": case "armor": case "shield": case "xp": case "level": case "cooldown": case "ability": case "skill": case "talent": case "perk":
                    return new[] { "Prefab Icon", "ScriptableObject Icon" };
                case "weapon": case "gun": case "sword": case "bow": case "ammo": case "reload": case "projectile": case "bullet": case "grenade": case "explosion":
                    return new[] { "Prefab Icon", "ParticleSystem Icon" };
                case "loot": case "drop": case "pickup": case "powerup": return new[] { "Prefab Icon" };
                case "inventory": case "bag": case "equip": case "equipment": case "slot": case "item": case "stack":
                    return new[] { "ScriptableObject Icon", "Prefab Icon" };
                case "craft": case "recipe": case "smith": case "alchemy": return new[] { "ScriptableObject Icon", "TextAsset Icon" };
                case "quest": case "mission": case "task": case "objective": case "goal":
                    return new[] { "ScriptableObject Icon", "TextAsset Icon", "Prefab Icon" };
                case "dialog": case "dialogue": case "convo": case "cutscene":
                    return new[] { "TextAsset Icon", "TimelineAsset Icon" };

                // World / Grids / Tiles / Spawning
                case "spawner": case "spawn": case "wave": case "pool": case "pooler": case "factory": return new[] { "Prefab Icon" };
                case "grid": case "tile": case "tilemap": return new[] { "Grid Icon", "Tilemap Icon" };
                case "proc": case "procedural": case "noise": case "perlin": return new[] { "ScriptableObject Icon" };

                // Systems / Config / Tools
                case "manager": case "system": case "service": case "provider": return new[] { "Settings Icon", "ScriptableObject Icon" };
                case "config": case "settings": case "options": case "profile": return new[] { "Settings Icon" };
                case "editor": case "tool": case "utility": case "debug": case "profiler": case "dev": case "cheat": case "console":
                    return new[] { "EditorWindow Icon", "Settings Icon", "ScriptableObject Icon" };
            }
            return Array.Empty<string>();
        }

        private static IEnumerable<string> GetTokens(Type t)
        {
            foreach (var tok in SplitPascalCase(t.Name)) yield return tok;
            if (!string.IsNullOrEmpty(t.Namespace))
            {
                foreach (var ns in t.Namespace.Split('.'))
                    foreach (var tok in SplitPascalCase(ns))
                        yield return tok;
            }
        }

        private static IEnumerable<string> SplitPascalCase(string s)
        {
            if (string.IsNullOrEmpty(s)) yield break;
            foreach (Match m in Regex.Matches(s, @"([A-Z]+(?![a-z]))|([A-Z]?[a-z]+)|(\d+)"))
                yield return m.Value.ToLowerInvariant();
        }

        private static string ToPascal(string token)
            => string.IsNullOrEmpty(token) ? token : char.ToUpperInvariant(token[0]) + token.Substring(1);
    }
}
#endif
