using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Jungle.Editor
{
   
    /// <summary>
    /// Editor utility window that scaffolds value wrapper classes and assets for
    /// custom data types using configurable generation options.
    /// </summary>
    public class ValueWrapperGeneratorWindow : EditorWindow
    {
        private static readonly Dictionary<string, Type> TypeAliases = new(StringComparer.OrdinalIgnoreCase)
        {
            { "bool", typeof(bool) },
            { "byte", typeof(byte) },
            { "sbyte", typeof(sbyte) },
            { "char", typeof(char) },
            { "decimal", typeof(decimal) },
            { "double", typeof(double) },
            { "float", typeof(float) },
            { "int", typeof(int) },
            { "uint", typeof(uint) },
            { "long", typeof(long) },
            { "ulong", typeof(ulong) },
            { "short", typeof(short) },
            { "ushort", typeof(ushort) },
            { "string", typeof(string) },
            { "object", typeof(object) }
        };

        private static readonly Dictionary<Type, string> AliasByType = TypeAliases.ToDictionary(pair => pair.Value, pair => pair.Key);

        private const string WindowUxmlPath = "Packages/jungle.core/Editor/Windows/Resources/ValueWrapperGenerator.uxml";
        private const string EditorStylesPath = "Packages/jungle.core/Editor/Resources/JungleEditorStyles.uss";

        private readonly List<string> generatedFiles = new();

        [SerializeField]
        private MonoScript scriptType;

        [SerializeField]
        private DefaultAsset outputFolder;

        [SerializeField]
        private string typeNameInput = string.Empty;

        [SerializeField]
        private string wrapperName = string.Empty;

        [SerializeField]
        private string wrappersNamespace = "Jungle.Values.Custom";

        [SerializeField]
        private string assetMenuPath = "Jungle/Values/Custom";

        [SerializeField]
        private string assetFileName = "CustomValue";

        [SerializeField]
        private bool generateInterfaceAndLocalValue = true;

        [SerializeField]
        private bool generateComponentWrapper = true;

        [SerializeField]
        private bool generateAssetWrapper = true;

        [SerializeField]
        private bool generateArrayWrapper = true;

        private ObjectField scriptTypeField;
        private TextField typeNameField;
        private TextField wrapperNameField;
        private TextField namespaceField;
        private ObjectField outputFolderField;
        private TextField assetMenuField;
        private TextField assetFileField;
        private Toggle interfaceToggle;
        private Toggle componentToggle;
        private Toggle assetToggle;
        private Toggle arrayToggle;
        private Button generateButton;
        private HelpBox resolvedTypeHelpBox;
        private Button resolveTypeButton;

        private Type resolvedType;

        [MenuItem("Tools/Jungle/Values/Value Wrapper Generator")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<ValueWrapperGeneratorWindow>();
            wnd.titleContent = new GUIContent("Value Wrapper Generator");
            wnd.minSize = new Vector2(420, 320);
            wnd.Show();
        }

        public void CreateGUI()
        {
            rootVisualElement.Clear();

            var visualTree = Resources.Load<VisualTreeAsset>("ValueWrapperGenerator");
            Assert.IsNotNull(
                visualTree,
                "ValueWrapperGenerator.uxml not found. Make sure it is inside a Resources folder and named 'ValueWrapperGenerator.uxml'."
            );

            if (visualTree != null)
                visualTree.CloneTree(rootVisualElement);

            var editorStyles = Resources.Load<StyleSheet>("JungleEditorStyles");
            if (editorStyles != null)
                rootVisualElement.styleSheets.Add(editorStyles);

            CacheControls();
            BindInitialValues();
            RegisterCallbacks();
            ResolveType();
            UpdateAssetFieldsState();
            UpdateGenerateButtonState();
        }

        private void CacheControls()
        {
            scriptTypeField = rootVisualElement.Q<ObjectField>("script-type-field");
            typeNameField = rootVisualElement.Q<TextField>("type-name-field");
            wrapperNameField = rootVisualElement.Q<TextField>("wrapper-name-field");
            namespaceField = rootVisualElement.Q<TextField>("namespace-field");
            outputFolderField = rootVisualElement.Q<ObjectField>("output-folder-field");
            assetMenuField = rootVisualElement.Q<TextField>("asset-menu-field");
            assetFileField = rootVisualElement.Q<TextField>("asset-file-field");
            interfaceToggle = rootVisualElement.Q<Toggle>("interface-toggle");
            componentToggle = rootVisualElement.Q<Toggle>("component-toggle");
            assetToggle = rootVisualElement.Q<Toggle>("asset-toggle");
            arrayToggle = rootVisualElement.Q<Toggle>("array-toggle");
            generateButton = rootVisualElement.Q<Button>("generate-button");
            resolvedTypeHelpBox = rootVisualElement.Q<HelpBox>("resolved-type-status");
            resolveTypeButton = rootVisualElement.Q<Button>("resolve-type-button");
        }

        private void BindInitialValues()
        {
            if (scriptTypeField != null)
            {
                scriptTypeField.objectType = typeof(MonoScript);
                scriptTypeField.allowSceneObjects = false;
                scriptTypeField.SetValueWithoutNotify(scriptType);
            }

            if (typeNameField != null)
            {
                typeNameField.SetValueWithoutNotify(typeNameInput);
            }

            if (wrapperNameField != null)
            {
                wrapperNameField.SetValueWithoutNotify(wrapperName);
            }

            if (namespaceField != null)
            {
                namespaceField.SetValueWithoutNotify(wrappersNamespace);
            }

            if (outputFolderField != null)
            {
                outputFolderField.objectType = typeof(DefaultAsset);
                outputFolderField.allowSceneObjects = false;
                outputFolderField.SetValueWithoutNotify(outputFolder);
            }

            if (assetMenuField != null)
            {
                assetMenuField.SetValueWithoutNotify(assetMenuPath);
            }

            if (assetFileField != null)
            {
                assetFileField.SetValueWithoutNotify(assetFileName);
            }

            interfaceToggle?.SetValueWithoutNotify(generateInterfaceAndLocalValue);
            componentToggle?.SetValueWithoutNotify(generateComponentWrapper);
            assetToggle?.SetValueWithoutNotify(generateAssetWrapper);
            arrayToggle?.SetValueWithoutNotify(generateArrayWrapper);
        }

        private void RegisterCallbacks()
        {
            if (scriptTypeField != null)
            {
                scriptTypeField.RegisterValueChangedCallback(evt =>
                {
                    scriptType = evt.newValue as MonoScript;
                    resolvedType = scriptType != null ? scriptType.GetClass() : null;
                    if (resolvedType != null)
                    {
                        typeNameInput = resolvedType.FullName;
                        typeNameField?.SetValueWithoutNotify(typeNameInput);
                        SetWrapperNameIfEmpty(resolvedType);
                    }

                    UpdateGenerateButtonState();
                });
            }

            if (typeNameField != null)
            {
                typeNameField.RegisterValueChangedCallback(evt =>
                {
                    typeNameInput = evt.newValue;
                    resolvedType = null;
                    UpdateGenerateButtonState();
                });
            }

            if (wrapperNameField != null)
            {
                wrapperNameField.RegisterValueChangedCallback(evt =>
                {
                    wrapperName = evt.newValue;
                    UpdateGenerateButtonState();
                });
            }

            if (namespaceField != null)
            {
                namespaceField.RegisterValueChangedCallback(evt =>
                {
                    wrappersNamespace = evt.newValue;
                    UpdateGenerateButtonState();
                });
            }

            if (outputFolderField != null)
            {
                outputFolderField.RegisterValueChangedCallback(evt =>
                {
                    outputFolder = evt.newValue as DefaultAsset;
                    UpdateGenerateButtonState();
                });
            }

            if (assetMenuField != null)
            {
                assetMenuField.RegisterValueChangedCallback(evt => { assetMenuPath = evt.newValue; });
            }

            if (assetFileField != null)
            {
                assetFileField.RegisterValueChangedCallback(evt => { assetFileName = evt.newValue; });
            }

            if (interfaceToggle != null)
            {
                interfaceToggle.RegisterValueChangedCallback(evt =>
                {
                    generateInterfaceAndLocalValue = evt.newValue;
                    UpdateGenerateButtonState();
                });
            }

            if (componentToggle != null)
            {
                componentToggle.RegisterValueChangedCallback(evt =>
                {
                    generateComponentWrapper = evt.newValue;
                    UpdateGenerateButtonState();
                });
            }

            if (assetToggle != null)
            {
                assetToggle.RegisterValueChangedCallback(evt =>
                {
                    generateAssetWrapper = evt.newValue;
                    UpdateAssetFieldsState();
                    UpdateGenerateButtonState();
                });
            }

            if (arrayToggle != null)
            {
                arrayToggle.RegisterValueChangedCallback(evt =>
                {
                    generateArrayWrapper = evt.newValue;
                    UpdateGenerateButtonState();
                });
            }

            resolveTypeButton?.RegisterCallback<ClickEvent>(_ =>
            {
                var type = ResolveType();
                if (type != null)
                {
                    SetWrapperNameIfEmpty(type);
                }
                UpdateGenerateButtonState();
            });

            if (generateButton != null)
            {
                generateButton.clicked += () =>
                {
                    Generate();
                    UpdateGenerateButtonState();
                };
            }
        }

        private void UpdateResolvedTypeDisplay()
        {
            if (resolvedTypeHelpBox == null)
            {
                return;
            }

            if (resolvedType != null)
            {
                resolvedTypeHelpBox.text = $"Resolved type: {resolvedType.FullName}";
                resolvedTypeHelpBox.messageType = HelpBoxMessageType.Info;
            }
            else if (!string.IsNullOrEmpty(typeNameInput))
            {
                resolvedTypeHelpBox.text = "Type has not been resolved. Provide a fully qualified name or assign a script.";
                resolvedTypeHelpBox.messageType = HelpBoxMessageType.Warning;
            }
            else
            {
                resolvedTypeHelpBox.text = "Enter a type name or assign a script to resolve the value type.";
                resolvedTypeHelpBox.messageType = HelpBoxMessageType.None;
            }
        }

        private void UpdateAssetFieldsState()
        {
            var enabled = generateAssetWrapper;
            assetMenuField?.SetEnabled(enabled);
            assetFileField?.SetEnabled(enabled);
        }

        private void UpdateGenerateButtonState()
        {
            generateButton?.SetEnabled(CanGenerate());
            UpdateResolvedTypeDisplay();
        }

        private void SetWrapperNameIfEmpty(Type type)
        {
            if (!string.IsNullOrEmpty(wrapperName) || type == null)
            {
                return;
            }

            wrapperName = SuggestWrapperName(type);
            SyncWrapperNameField();
        }

        private void SyncWrapperNameField()
        {
            if (wrapperNameField == null)
            {
                return;
            }

            if (!string.Equals(wrapperNameField.value, wrapperName, StringComparison.Ordinal))
            {
                wrapperNameField.SetValueWithoutNotify(wrapperName);
            }
        }

        private bool CanGenerate()
        {
            return ResolveType() != null
                && !string.IsNullOrWhiteSpace(wrapperName)
                && !string.IsNullOrWhiteSpace(wrappersNamespace)
                && outputFolder != null
                && (generateInterfaceAndLocalValue || generateComponentWrapper || generateAssetWrapper || generateArrayWrapper);
        }

        private Type ResolveType()
        {
            if (resolvedType != null)
            {
                return resolvedType;
            }

            if (scriptType != null)
            {
                resolvedType = scriptType.GetClass();
                if (resolvedType != null)
                {
                    return resolvedType;
                }
            }

            var candidate = typeNameInput?.Trim();
            if (string.IsNullOrEmpty(candidate))
            {
                return null;
            }

            if (TypeAliases.TryGetValue(candidate, out var aliasType))
            {
                resolvedType = aliasType;
                SetWrapperNameIfEmpty(resolvedType);

                return resolvedType;
            }

            resolvedType = Type.GetType(candidate);
            if (resolvedType != null)
            {
                SetWrapperNameIfEmpty(resolvedType);

                return resolvedType;
            }

            resolvedType = Type.GetType($"{candidate}, UnityEngine.CoreModule");
            if (resolvedType != null)
            {
                SetWrapperNameIfEmpty(resolvedType);

                return resolvedType;
            }

            resolvedType = Type.GetType($"{candidate}, UnityEngine");
            if (resolvedType != null)
            {
                SetWrapperNameIfEmpty(resolvedType);

                return resolvedType;
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                resolvedType = assembly.GetType(candidate, false);
                if (resolvedType != null)
                {
                    SetWrapperNameIfEmpty(resolvedType);

                    return resolvedType;
                }
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException exception)
                {
                    types = exception.Types.Where(t => t != null).ToArray();
                }

                foreach (var type in types)
                {
                    if (type.Name == candidate)
                    {
                        resolvedType = type;
                        SetWrapperNameIfEmpty(resolvedType);

                        return resolvedType;
                    }
                }
            }

            return resolvedType;
        }

        private void Generate()
        {
            var type = ResolveType();
            if (type == null)
            {
                EditorUtility.DisplayDialog("Value Wrapper Generator", "Unable to resolve the provided type.", "OK");
                return;
            }

            var folderPath = AssetDatabase.GetAssetPath(outputFolder);
            if (string.IsNullOrEmpty(folderPath))
            {
                EditorUtility.DisplayDialog("Value Wrapper Generator", "Select a valid output folder.", "OK");
                return;
            }

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            generatedFiles.Clear();

            var interfaceName = $"I{wrapperName}Value";
            var localValueClass = $"{wrapperName}Value";
            var componentClass = $"{wrapperName}ValueComponent";
            var fromComponentClass = $"{wrapperName}ValueFromComponent";
            var assetClass = $"{wrapperName}ValueAsset";
            var fromAssetClass = $"{wrapperName}ValueFromAsset";
            var arrayClass = $"{wrapperName}LocalArrayValue";

            if (generateInterfaceAndLocalValue)
            {
                var filePath = Path.Combine(folderPath, $"{wrapperName}Value.cs");
                var content = BuildInterfaceAndLocalValueFile(type, interfaceName, localValueClass);
                WriteFile(filePath, content);
            }

            if (generateComponentWrapper)
            {
                var filePath = Path.Combine(folderPath, $"{wrapperName}ValueComponent.cs");
                var content = BuildComponentFile(type, interfaceName, componentClass, fromComponentClass);
                WriteFile(filePath, content);
            }

            if (generateAssetWrapper)
            {
                if (string.IsNullOrWhiteSpace(assetFileName))
                {
                    assetFileName = $"{wrapperName}Value";
                }

                var filePath = Path.Combine(folderPath, $"{wrapperName}ValueAsset.cs");
                var content = BuildAssetFile(type, interfaceName, assetClass, fromAssetClass);
                WriteFile(filePath, content);
            }

            if (generateArrayWrapper)
            {
                var filePath = Path.Combine(folderPath, $"{wrapperName}LocalArrayValue.cs");
                var content = BuildArrayFile(type, interfaceName, arrayClass);
                WriteFile(filePath, content);
            }

            AssetDatabase.Refresh();

            var message = generatedFiles.Count == 0
                ? "No files were generated."
                : $"Generated files:{Environment.NewLine}{string.Join(Environment.NewLine, generatedFiles)}";

            EditorUtility.DisplayDialog("Value Wrapper Generator", message, "OK");
        }

        private void WriteFile(string path, string content)
        {
            File.WriteAllText(path, content, Encoding.UTF8);
            generatedFiles.Add(path);
        }

        private string BuildInterfaceAndLocalValueFile(Type type, string interfaceName, string localValueClass)
        {
            var usings = CollectUsings(type, includeUnity: false, includeJungleValues: true);
            var typeName = GetTypeDisplayName(type);

            var sb = new StringBuilder();
            AppendUsings(sb, usings);

            sb.AppendLine();
            sb.AppendLine($"namespace {wrappersNamespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public interface {interfaceName} : IValue<{typeName}>");
            sb.AppendLine("    {");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    [Serializable]");
            sb.AppendLine($"    public class {localValueClass} : LocalValue<{typeName}>, {interfaceName}");
            sb.AppendLine("    {");
            sb.AppendLine("        public override bool HasMultipleValues => false;");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string BuildComponentFile(Type type, string interfaceName, string componentClass, string fromComponentClass)
        {
            var usings = CollectUsings(type, includeUnity: true, includeJungleValues: true);
            var typeName = GetTypeDisplayName(type);

            var sb = new StringBuilder();
            AppendUsings(sb, usings);

            sb.AppendLine();
            sb.AppendLine($"namespace {wrappersNamespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {componentClass} : ValueComponent<{typeName}>");
            sb.AppendLine("    {");
            sb.AppendLine("        [SerializeField]");
            sb.AppendLine($"        private {typeName} value;");
            sb.AppendLine();
            sb.AppendLine($"        public override {typeName} Value()");
            sb.AppendLine("        {");
            sb.AppendLine("            return value;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    [Serializable]");
            sb.AppendLine($"    public class {fromComponentClass} : ValueFromComponent<{typeName}, {componentClass}>, {interfaceName}");
            sb.AppendLine("    {");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string BuildAssetFile(Type type, string interfaceName, string assetClass, string fromAssetClass)
        {
            var usings = CollectUsings(type, includeUnity: true, includeJungleValues: true);
            var typeName = GetTypeDisplayName(type);

            var sb = new StringBuilder();
            AppendUsings(sb, usings);

            sb.AppendLine();
            sb.AppendLine($"namespace {wrappersNamespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    [CreateAssetMenu(menuName = \"{assetMenuPath}\", fileName = \"{assetFileName}\")]");
            sb.AppendLine($"    public class {assetClass} : ValueAsset<{typeName}>");
            sb.AppendLine("    {");
            sb.AppendLine("        [SerializeField]");
            sb.AppendLine($"        private {typeName} value;");
            sb.AppendLine();
            sb.AppendLine($"        public override {typeName} Value()");
            sb.AppendLine("        {");
            sb.AppendLine("            return value;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    [Serializable]");
            sb.AppendLine($"    public class {fromAssetClass} : ValueFromAsset<{typeName}, {assetClass}>, {interfaceName}");
            sb.AppendLine("    {");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string BuildArrayFile(Type type, string interfaceName, string arrayClass)
        {
            var usings = CollectUsings(type, includeUnity: false, includeJungleValues: true);
            var typeName = GetTypeDisplayName(type);

            var sb = new StringBuilder();
            AppendUsings(sb, usings);

            sb.AppendLine();
            sb.AppendLine($"namespace {wrappersNamespace}");
            sb.AppendLine("{");
            sb.AppendLine("    [Serializable]");
            sb.AppendLine($"    public class {arrayClass} : LocalArrayValue<{typeName}>, {interfaceName}");
            sb.AppendLine("    {");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private IReadOnlyList<string> CollectUsings(Type type, bool includeUnity, bool includeJungleValues)
        {
            var namespaces = new HashSet<string>();

            namespaces.Add("System");
            if (includeUnity)
            {
                namespaces.Add("UnityEngine");
            }

            if (includeJungleValues)
            {
                namespaces.Add("Jungle.Values");
            }

            var typeNamespace = type.Namespace;
            if (!string.IsNullOrWhiteSpace(typeNamespace))
            {
                namespaces.Add(typeNamespace);
            }

            return namespaces
                .OrderBy(n => n, StringComparer.Ordinal)
                .ToList();
        }

        private static void AppendUsings(StringBuilder sb, IEnumerable<string> namespaces)
        {
            foreach (var ns in namespaces)
            {
                sb.Append("using ");
                sb.Append(ns);
                sb.AppendLine(";");
            }
        }

        private static string SuggestWrapperName(Type type)
        {
            if (type == null)
            {
                return string.Empty;
            }

            if (AliasByType.TryGetValue(type, out var alias))
            {
                return char.ToUpperInvariant(alias[0]) + alias.Substring(1);
            }

            if (type.IsGenericType)
            {
                var baseName = type.Name.Substring(0, type.Name.IndexOf('`'));
                return baseName;
            }

            if (type.IsArray)
            {
                var elementName = SuggestWrapperName(type.GetElementType());
                return string.IsNullOrEmpty(elementName) ? type.Name : $"{elementName}Array";
            }

            return type.Name;
        }

        private static string GetTypeDisplayName(Type type)
        {
            if (AliasByType.TryGetValue(type, out var alias))
            {
                return alias;
            }

            if (type.IsArray)
            {
                var elementType = GetTypeDisplayName(type.GetElementType());
                return $"{elementType}[]";
            }

            if (type.IsGenericType)
            {
                var baseName = type.Name.Substring(0, type.Name.IndexOf('`'));
                var genericArgs = type.GetGenericArguments().Select(GetTypeDisplayName);
                return $"{baseName}<{string.Join(", ", genericArgs)}>";
            }

            return type.Name;
        }
    }
}

