// // SpriteColorAction2D.cs
//
// using System.Collections.Generic;
// using Jungle.Actions;
// using Jungle.Core;
// using Jungle.DragContext;
// using UnityEngine;
//
// namespace Jungle.Actions
// {
//     [System.Serializable]
//     public class SpriteColorAction2D : ProcessAction
//     {
//         [SerializeReference] private FlexibleSpriteRendererValue targetRenderer = new();
//         [SerializeField] private Color highlightColor = Color.yellow;
//         [SerializeField] private bool useAlphaFade;
//         [SerializeField] private float fadeSpeed = 5f;
//
//         private Dictionary<SpriteRenderer, Color> originalColors = new ();
//         private bool isHighlighted;
//         private bool skipStop;
//         
//
//         public void StartAction()
//         {
//             Start();
//         }
//
//         public void StopAction()
//         {
//             Stop();
//         }
//
//         private void Update()
//         {
//             if (!isHighlighted) return;
//
//             foreach (var renderer in targetRenderer.Value)
//             {
//                 if (renderer == null) continue;
//
//                 if (useAlphaFade && originalColors.ContainsKey(renderer))
//                 {
//                     Color currentColor = renderer.color;
//                     float alpha = Mathf.PingPong(Time.time * fadeSpeed, 1f);
//                     currentColor.a = Mathf.Lerp(originalColors[renderer].a * 0.3f, originalColors[renderer].a, alpha);
//                     renderer.color = currentColor;
//                 }
//             }
//         }
//
//         protected override void BeginImpl()
//         {
//             originalColors.Clear();
//             foreach (var renderer in targetRenderer.Value)
//             {
//                 if (renderer != null)
//                 {
//                     originalColors[renderer] = renderer.color;
//                 }
//             }
//             
//             bool anyRendererFound = false;
//             foreach (var renderer in targetRenderer.Value)
//             {
//                 if (renderer != null)
//                 {
//                     renderer.color = highlightColor;
//                     anyRendererFound = true;
//                 }
//             }
//             isHighlighted = anyRendererFound;
//         }
//
//         protected override void CompleteImpl()
//         {
//             if (skipStop)
//             {
//                 return;
//             }
//
//             foreach (var renderer in targetRenderer.Value)
//             {
//                 if (renderer != null && originalColors.ContainsKey(renderer))
//                 {
//                     renderer.color = originalColors[renderer];
//                 }
//             }
//             isHighlighted = false;
//         }
//         
//     }
// }
